﻿using System.Text.Json;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Configuration;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.RPC.Eth.DTOs;
using Voting.Server.Persistence;
using Voting.Server.Persistence.Accounts;
using Voting.Server.Persistence.Clients;
using Voting.Server.Persistence.ContractDefinition;
using Voting.Server.Protos.v1;
using Voting.Server.Tests.Integration.TestNet.Ganache;
using Voting.Server.Tests.Utils;
using Voting.Server.UnitTests;
using Voting.Server.Utils.Mappings;
using static NUnit.Framework.TestContext;

namespace Voting.Server.Tests.Integration;

// [Ignore("Debug")]
[Order(4)]
[TestFixture]
public class VotingDbRepositoryTests__ReadVotesByCandidateAndSectionAsync : IUseBlockchainAndRepositoryProps
{
    public IGanache TestNet { get; set; } = default!;
    public IConfiguration Config { get; set; } = default!;
    public AccountManager AccountManager { get; set; } = default!;
    public IWeb3ClientsManager ClientsManager { get; set; } = default!;
    public IVotingDbRepository Repository { get; set; } = default!;
    public string Account { get; set; } = default!;
    private readonly SeedDataBuilder _seedDataBuilder = new();

    [Order(1)]
    [Test, Sequential]
    public async Task ReadVotesByCandidateAndSectionAsync_Should_Return_Correct_Data(
        [Values(10U, 20U, 30U, 50U, 100U)] uint numSections,
        [Values(3U, 4U, 5U, 7U, 10U)] uint numCandidates)
    {
        //Generate seed data.
        SeedData seedData = _seedDataBuilder.GenerateNew(numSections, numCandidates);
        
        //Deploy Contract
        TransactionReceipt transaction = await Repository.CreateSectionRange(seedData.Deployment);
        
        //Check BYTECODE and transaction status.
        Guard.IsNotNullOrEmpty(await Repository.Web3.Eth.GetCode.SendRequestAsync(transaction.ContractAddress));
        Guard.IsEqualTo(transaction.Status.ToLong(), 1);

        //Get valid random Section
        Section? expectedSection = seedData.Sections.MinBy(_ => Guid.NewGuid());
        Guard.IsNotNull(expectedSection);
        
        //Get valid random CandidateVotes
        CandidateVotes? expectedCandidateVotes = expectedSection.CandidateVotes.MinBy(_ => Guid.NewGuid());
        Guard.IsNotNull(expectedCandidateVotes);
        
        //Remove other CandidateVotes from expectedSection
        List<CandidateVotes> cvToRemove = new List<CandidateVotes>(expectedSection.CandidateVotes);
        cvToRemove = cvToRemove.Where(cv => cv.Candidate != expectedCandidateVotes.Candidate).ToList();
        cvToRemove.ForEach(cv => expectedSection.CandidateVotes.Remove(cv));

        //Prints SectionID and Candidate
        WriteLine($"Trying to access contract and getting section {expectedSection.SectionID}...");
        WriteLine($"Trying to access contract and getting candidate {expectedCandidateVotes.Candidate}...");

        //Calls method and convert results to JSON.
        CandidateEventDTO? resultCandidateEventDTO = 
            await Repository.ReadVotesByCandidateAndSectionAsync(
                expectedCandidateVotes.Candidate, 
                expectedSection.SectionID, 
                FilterRange.FromLatestToLatest);
        Section resultSection = Mappings.CandidateEventDTOToSection(resultCandidateEventDTO);
        
        string resultJSON = JsonSerializer.Serialize(resultSection);
        string expectedJSON = JsonSerializer.Serialize(expectedSection);
        WriteLine(resultJSON);
        WriteLine("Expected: " + expectedJSON);

        //Assertions.
        Assert.That(resultJSON, Is.EqualTo(expectedJSON));
        Assert.That(resultSection, Is.Not.SameAs(expectedSection));
        Assert.That(resultSection.CandidateVotes, Is.Not.SameAs(expectedSection.CandidateVotes));
        Assert.That(resultSection.SectionID, Is.EqualTo(expectedSection.SectionID));
        Assert.That(resultSection.CandidateVotes.First(), Is.EqualTo(expectedCandidateVotes));
    }

    [Order(2)]
    [Test, Sequential]
    public async Task  ReadVotesByCandidateAndSectionAsync_Should_Return_Null_When_Candidate_Or_Section_Doesnt_Exist(
        [Values(10U, 20U, 30U, 50U, 100U)] uint numSections,
        [Values(3U, 4U, 5U, 7U, 10U)] uint numCandidates)
    {
        //Generate seed data.
        SeedData seedData = _seedDataBuilder.GenerateNew(numSections, numCandidates);
        
        //Deploy Contract
        TransactionReceipt transaction = await Repository.CreateSectionRange(seedData.Deployment);
        
        //Check BYTECODE and transaction status.
        Guard.IsNotNullOrEmpty(await Repository.Web3.Eth.GetCode.SendRequestAsync(transaction.ContractAddress));
        Guard.IsEqualTo(transaction.Status.ToLong(), 1);

        uint invalidSectionNumber = CurrentContext.Random.NextUInt(SeedDataBuilder.MaxSectionID, uint.MaxValue - 1);
        uint invalidCandidateNumber = CurrentContext.Random.NextUInt(SeedDataBuilder.MaxCandidateNumber, uint.MaxValue - 1);

        //Calls method with invalid candidate.
        CandidateEventDTO? resultInvalidCandidate = await Repository.ReadVotesByCandidateAndSectionAsync(
            invalidCandidateNumber, seedData.Deployment.Sections.First(), FilterRange.FromLatestToLatest);

        //Calls method with invalid section.
        CandidateEventDTO? resultInvalidSection = await Repository.ReadVotesByCandidateAndSectionAsync(
            seedData.Deployment.Candidates.First(), invalidSectionNumber, FilterRange.FromLatestToLatest);

        //Calls method with invalid candidate and section.
        CandidateEventDTO? resultInvalidCandidateAndSection = await Repository.ReadVotesByCandidateAndSectionAsync(
            invalidCandidateNumber, invalidSectionNumber, FilterRange.FromLatestToLatest);

        //Assertions.
        Assert.That(resultInvalidCandidate, Is.Null);
        Assert.That(resultInvalidSection, Is.Null);
        Assert.That(resultInvalidCandidateAndSection, Is.Null);
    }

    [Order(3)]
    [Test]
    public void  ReadVotesByCandidateAndSectionAsync_Should_Throw_Exception_When_CandidateNum_Or_SectionNum_Is_Zero_Or_No_Param()
    {
        //Assertions.
        Assert.That(async () => await Repository.ReadVotesByCandidateAndSectionAsync(), 
            Throws.InstanceOf<ArgumentException>());
        Assert.That(async () => await Repository.ReadVotesByCandidateAndSectionAsync(0), 
            Throws.InstanceOf<ArgumentException>());
        Assert.That(async () => await Repository.ReadVotesByCandidateAndSectionAsync(0, 0), 
            Throws.InstanceOf<ArgumentException>());
    }
}