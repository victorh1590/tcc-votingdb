﻿using System.Text.Json;
using Voting.Server.Protos.v1;
using Voting.Server.Tests.Utils;
using static NUnit.Framework.TestContext;

namespace Voting.Server.Tests.Unit;

public partial class DomainServiceTests
{
    [Test]
    [Repeat(10)]
    public async Task GetSectionAsync_Should_Return_Correct_Data_When_All_SectionNums_Are_Valid()
    {
        //Select a valid expected section.
        Section expectedSection = _seedData.Sections
            .OrderBy(_ => Guid.NewGuid())
            .First();
        
        //Calls method and convert results to JSON.
        Section resultSection = await _domainService.GetSectionAsync(expectedSection.SectionID);

        string resultJSON = JsonSerializer.Serialize(resultSection);
        string expectedJSON = JsonSerializer.Serialize(expectedSection);

        //Assertions
        Assert.That(resultJSON, Is.EqualTo(expectedJSON));
        Assert.That(resultSection, Is.Not.SameAs(expectedSection));
        Assert.That(resultSection.CandidateVotes, Is.Not.SameAs(expectedSection.CandidateVotes));
        Assert.That(resultSection.CandidateVotes, Is.EquivalentTo(expectedSection.CandidateVotes));
        Assert.That(resultSection.SectionID, Is.EqualTo(expectedSection.SectionID));
    }
    
    [Test]
    [Repeat(5)]
    public void GetSectionAsync_Should_Fail_When_SectionID_Is_Invalid()
    {
        Assert.That(async () => await _domainService.GetSectionAsync(
                CurrentContext.Random.NextUInt(SeedDataBuilder.MaxSectionID, uint.MaxValue - 1)), 
            Throws.TypeOf<ArgumentException>().Or.TypeOf<ArgumentNullException>());
    }
}