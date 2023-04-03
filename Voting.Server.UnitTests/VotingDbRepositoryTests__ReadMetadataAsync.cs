﻿using Microsoft.Extensions.Configuration;
using Voting.Server.Persistence.Accounts;
using Voting.Server.Persistence.Clients;
using Voting.Server.Persistence;
using Voting.Server.UnitTests.TestNet.Ganache;
using CommunityToolkit.Diagnostics;
using Nethereum.RPC.Eth.DTOs;
using Voting.Server.UnitTests.TestData;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Voting.Server.Persistence.ContractDefinition;
using Nethereum.Util;

namespace Voting.Server.UnitTests;

[Order(5)]
[TestFixture]
public class VotingDbRepositoryTests__ReadMetadataAsync : IUseBlockchainAndRepositoryProps
{
    public TestNet<Ganache> TestNet { get; set; } = default!;
    public IConfiguration Config { get; set; } = default!;
    public AccountManager AccountManager { get; set; } = default!;
    public IWeb3ClientsManager ClientsManager { get; set; } = default!;
    public IVotingDbRepository Repository { get; set; } = default!;
    public string Account { get; set; } = default!;

    [Order(1)]
    [Test, Sequential]
    public async Task ReadMetadataAsync_Should_Return_Correct_Data(
        [Values(10U, 20U, 30U, 50U, 100U)] uint numSections,
        [Values(3U, 4U, 5U, 7U, 10U)] uint numCandidates)
    {
        //Generate seed data.
        SeedData seedData = SeedDataBuilder.GenerateNew(numSections, numCandidates);

        //Deploy Contract
        TransactionReceipt transaction = await Repository.CreateSectionRange(seedData.Deployment);
        TestContext.WriteLine("Contract Address: " + transaction.ContractAddress);

        //Check BYTECODE and transaction status.
        Guard.IsNotNullOrEmpty(await Repository.Web3.Eth.GetCode.SendRequestAsync(transaction.ContractAddress));
        Guard.IsEqualTo(transaction.Status.ToLong(), 1);

        //Calls method passing valid address.
        MetadataEventDTO? metadataEventDTO = await Repository.ReadMetadataAsync(transaction.ContractAddress, FilterRange.FromLatestToLatest);

        //Assertions.
        Assert.That(metadataEventDTO, Is.Not.Null);
        Assert.That(metadataEventDTO.ContractAddress.ToLowerInvariant(), Is.EqualTo(transaction.ContractAddress));
        Assert.That(metadataEventDTO.Block.ToString(), Is.EqualTo(transaction.BlockNumber.ToString()));
        CollectionAssert.AreEquivalent(metadataEventDTO.Sections, seedData.Deployment.Sections);
    }

    [Order(2)]
    [Test, Sequential]
    public async Task ReadMetadataAsync_Returns_Null_When_Contract_Doesnt_Exist(
    [Values(10U, 20U, 30U, 50U, 100U)] uint numSections,
    [Values(3U, 4U, 5U, 7U, 10U)] uint numCandidates)
    {
        //Generate seed data.
        SeedData seedData = SeedDataBuilder.GenerateNew(numSections, numCandidates);

        //Deploy Contract
        TransactionReceipt transaction = await Repository.CreateSectionRange(seedData.Deployment);
        TestContext.WriteLine("Contract Address: " + transaction.ContractAddress);

        //Check BYTECODE and transaction status.
        Guard.IsNotNullOrEmpty(await Repository.Web3.Eth.GetCode.SendRequestAsync(transaction.ContractAddress));
        Guard.IsEqualTo(transaction.Status.ToLong(), 1);

        //Generate fake address from random string hashed to Sha3 and converted to 20 bytes address.
        string randomString = default!;
        string fakeAddress = default!;
        Sha3Keccack kck = new();

        do
        {
            randomString = "0x" + TestContext.CurrentContext.Random.GetString(50, "abcdefghijkmnopqrstuvwxyz0123456789");
            fakeAddress = kck.CalculateHash(randomString).Remove(40);
            fakeAddress = Nethereum.Web3.Web3.ToValid20ByteAddress(fakeAddress);
        } while (fakeAddress == transaction.ContractAddress);

        TestContext.WriteLine("Fake address: " + fakeAddress);
        TestContext.WriteLine("Fake address length: " + fakeAddress.Length);

        //Calls method passing fake address.
        MetadataEventDTO? metadataEventDTO = await Repository.ReadMetadataAsync(fakeAddress, FilterRange.FromLatestToLatest);

        //Assertions
        Assert.That(metadataEventDTO, Is.Null);
    }
}