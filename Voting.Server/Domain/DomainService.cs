﻿using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using CommunityToolkit.Diagnostics;
using Voting.Server.Persistence;
using Voting.Server.Persistence.ContractDefinition;
using Nethereum.ABI;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Voting.Server.Domain.Models;
using Voting.Server.Domain.Utils;

namespace Voting.Server.Domain;

internal class DomainService
{
    private string URL { get; }
    private int ChainID { get; }
    private string PrivateKey { get; }

    public DomainService()
    {
        URL = "HTTP://localhost:7545";
        ChainID = 5777;
        PrivateKey = "d1d45d0629c5a5fe5ac563c89d759ca057cb7e2936f6c78351bb93bb2f58eb99";
    }

    private Web3 GetWeb3Client()
    {
        Account account = new Account(PrivateKey, 5777);
        return new Web3(account, URL);
    }

    public async Task<string> DeployContract(VotingDbDeployment votingDbDeployment)
    {
        Web3 web3 = GetWeb3Client();
        web3.TransactionManager.UseLegacyAsDefault = true;
        TransactionReceipt transactionReceipt = await web3.Eth
            .GetContractDeploymentHandler<VotingDbDeployment>()
            .SendRequestAndWaitForReceiptAsync(votingDbDeployment);
        return transactionReceipt.ContractAddress;
    }

    private VotingDbService GetVotingDbService(string contractAddress)
    {
        Web3 web3 = GetWeb3Client();
        return new VotingDbService(web3, contractAddress);
    }

    public async Task<SectionVotes> GetSectionAsync(uint sectionNumber = 0, Web3? web3 = null)
    {
        Guard.IsNotEqualTo(sectionNumber, 0);
        
        web3 ??= GetWeb3Client();
        Event<SectionEventDTO> sectionEventHandler = web3.Eth.GetEvent<SectionEventDTO>();
        BlockParameter earliest = BlockParameter.CreateEarliest();
        BlockParameter latest = BlockParameter.CreateLatest();
        NewFilterInput sectionEventFilter = sectionEventHandler.CreateFilterInput(sectionNumber, earliest, latest);
        List<EventLog<SectionEventDTO>>? sectionLogs = await sectionEventHandler.GetAllChangesAsync(sectionEventFilter);
        Guard.IsNotNull(sectionLogs);
        Guard.IsNotEmpty(sectionLogs);
        SectionEventDTO sectionEvent = sectionLogs.FirstOrDefault()!.Event;
        Guard.IsEqualTo(sectionEvent.Candidates.Count, sectionEvent.Votes.Count);

        Dictionary<uint, uint> votesByCandidate = new();
        for (int i = 0; i < sectionEvent.Candidates.Count; i++)
        {
            votesByCandidate.Add(sectionEvent.Candidates[i], sectionEvent.Votes[i]);
        }

        return new SectionVotes(sectionEvent.Section, votesByCandidate);
    }

    public async Task<List<SectionVotes>> GetSectionRangeAsync(uint[] sectionNumbers)
    {
        Guard.IsNotEmpty(sectionNumbers);
        
        Web3 web3 = GetWeb3Client();
        List<SectionVotes> sectionVotesList = new();
        foreach (var sectionNumber in sectionNumbers)
        {
            SectionVotes sectionVotes = await GetSectionAsync(sectionNumber, web3);
            sectionVotesList.Add(sectionVotes);
        }
        Guard.IsNotNull(sectionVotesList);
        Guard.IsNotEmpty(sectionVotesList);

        return sectionVotesList;
    }

    // public async Task GetVotesByCandidate(uint candidate = 0)
    // {
    //     //TODO return SectionVotes[i].Votes["Name"]
    // }
    //
    public async Task<dynamic> GetVotesByCandidateForSection(uint candidate = 0, uint sectionNumber = 0)
    {
        Guard.IsNotEqualTo(candidate, 0);
        Guard.IsNotEqualTo(sectionNumber, 0);
        
        Web3 web3 = GetWeb3Client();
        Event<CandidateEventDTO> candidateEventHandler = web3.Eth.GetEvent<CandidateEventDTO>();
        BlockParameter earliest = BlockParameter.CreateEarliest();
        BlockParameter latest = BlockParameter.CreateLatest();
        NewFilterInput candidateEventFilter = candidateEventHandler.CreateFilterInput(sectionNumber, earliest, latest);
        List<EventLog<CandidateEventDTO>>? candidateLogs = await candidateEventHandler.GetAllChangesAsync(candidateEventFilter);
        Guard.IsNotNull(candidateLogs);
        Guard.IsNotEmpty(candidateLogs);
        CandidateEventDTO candidateEvent = candidateLogs.FirstOrDefault()!.Event;
        
        return new { candidateEvent.Section, candidateEvent.Candidate, candidateEvent.Votes };
    }
    //
    // public async Task GetAllVotesByCandidate(string candidate)
    // {
    //     //TODO return Soma SectionVotes[i].Votes["Name"]
    // }
    //
    // public async Task GetAllVotes()
    // {
    //     //TODO return Soma SectionVotes[]
    // }
    //
    public async Task<string> CreateSection(SectionVotes section)
    {
        Guard.IsNotEqualTo(section.Section, 0);
        
        List<uint> candidates = section.Votes.Keys.ToList();
        Guard.IsNotEmpty(candidates);

        List<List<uint>> votes = new() { section.Votes.Values.ToList() };
        Guard.IsNotEmpty(votes[0]);
        Guard.IsNotEqualTo(votes[0].Count, candidates.Count);
        
        string sectionJSON = JsonSerializer.Serialize(section);
        Guard.IsNotNullOrEmpty(sectionJSON);

        Compression compression = new();
        string compressedSection = compression.Compress(sectionJSON);
        Guard.IsNotNullOrEmpty(compressedSection);

        DateTime currentTime = DateTime.Now;
        string timestamp = currentTime.ToString(CultureInfo.InvariantCulture);

        VotingDbDeployment deployment = new()
        {
            Votes = votes,
            Candidates = candidates,
            Sections = new List<uint> { section.Section },
            Timestamp = timestamp,
            CompressedSectionData = compressedSection
        };

        return await DeployContract(deployment);
    }
    //
    // public async Task CreateSectionRange()
    // {
    //     //TODO call deploy a contract with sections.
    // }
}