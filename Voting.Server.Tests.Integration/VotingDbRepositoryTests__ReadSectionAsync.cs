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
[Order(2)]
[TestFixture]
public class VotingDbRepositoryTests__ReadSectionAsync : IUseBlockchainAndRepositoryProps
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
    public async Task ReadSectionAsync_Should_Return_Correct_Data(
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

        //Get valid random section number.
        uint sectionNumber = seedData.Deployment.Sections.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
        WriteLine($"Trying to access contract and getting section {sectionNumber}...");
        Section? expectedSection = seedData.Sections
            .Select(section => section)
            .FirstOrDefault(section => section.SectionID == sectionNumber);
        Guard.IsNotNull(expectedSection);

        //Calls method and convert results to JSON.
        SectionEventDTO? resultSectionEventDTO = 
            await Repository.ReadSectionAsync(sectionNumber, FilterRange.FromLatestToLatest);
        Section resultSection = Mappings.SectionEventDTOToSection(resultSectionEventDTO);

        string resultJSON = JsonSerializer.Serialize(resultSection);
        string expectedJSON = JsonSerializer.Serialize(expectedSection);

        //Assertions.
        Assert.That(resultJSON, Is.EqualTo(expectedJSON));
        Assert.That(resultSection, Is.Not.SameAs(expectedSection));
        Assert.That(resultSection.CandidateVotes, Is.Not.SameAs(expectedSection.CandidateVotes));
        Assert.That(resultSection.SectionID, Is.EqualTo(expectedSection.SectionID));
        Assert.That(resultSection.CandidateVotes, Is.EqualTo(expectedSection.CandidateVotes));
    }
    
    [Order(2)]
    [Test, Sequential]
    public async Task ReadSectionAsync_Should_Return_Null_When_Looking_For_Invalid_Section(
        [Values(1U, 5U, 10U)] uint numSections,
        [Values(3U, 4U, 5U)] uint numCandidates)
    {
        //Generate seed data.
        SeedData seedData = _seedDataBuilder.GenerateNew(numSections, numCandidates);
        
        //Deploy Contract
        TransactionReceipt transaction = await Repository.CreateSectionRange(seedData.Deployment);
        
        //Check BYTECODE and transaction status.
        Guard.IsNotNullOrEmpty(await Repository.Web3.Eth.GetCode.SendRequestAsync(transaction.ContractAddress));
        Guard.IsEqualTo(transaction.Status.ToLong(), 1);
    
        //Get valid random section number NOT IN seedData.
        uint sectionNumber = CurrentContext.Random.NextUInt(SeedDataBuilder.MaxSectionID, uint.MaxValue - 1);
        WriteLine($"Trying to access contract and getting section {sectionNumber}...");
    
        //Calls method and convert results to JSON.
        SectionEventDTO? resultSectionEventDTO = 
            await Repository.ReadSectionAsync(sectionNumber, FilterRange.FromLatestToLatest);
        
        //Assertions.
        Assert.That(resultSectionEventDTO, Is.Null);
    }
    
    
    [Order(3)]
    [Test]
    public void ReadSectionAsync_Should_Throw_Exception_When_SectionNum_Is_Zero_Or_No_Param()
    {
        //Assertions.
        Assert.That(async () => await Repository.ReadSectionAsync(), Throws.InstanceOf<ArgumentException>());
        Assert.That(async () => await Repository.ReadSectionAsync(0), Throws.InstanceOf<ArgumentException>());
    }
}