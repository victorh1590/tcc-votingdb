﻿using System.Text.Json;
using Voting.Server.Protos.v1;
using Voting.Server.Tests.Utils;
using static NUnit.Framework.TestContext;

namespace Voting.Server.Tests.Unit;

public partial class DomainServiceTests
{
    [Test]
    [Repeat(10)]
    public async Task RemoveRedundantSectionsAsync_Should_Remove_Redundant_Sections_Correctly()
    {
        //Arrange
        SeedData seedData2 = _seedDataBuilder.GenerateNew(10U, 5U);
        List<Section> entrySeedData = new(seedData2.Sections);

        //Copy new seedData so we can keep track of the original data after changes
        List<Section> expectedSections = new(entrySeedData);
        
        //Mix existing sections with sections already inserted
        entrySeedData.AddRange(_seedData.Sections
            .OrderBy(_ => Guid.NewGuid())
            .Take(CurrentContext.Random.Next(0, _seedData.Sections.Count))
        );
        
        //Act
        List<Section> resultSections = await _domainService.RemoveRedundantSectionsAsync(entrySeedData);
        resultSections.Sort((x, y) => x.SectionID > y.SectionID ? 1 : -1 );
        expectedSections.Sort((x, y) => x.SectionID > y.SectionID ? 1 : -1);
        string resultJSON = JsonSerializer.Serialize(resultSections);
        string expectedJSON = JsonSerializer.Serialize(expectedSections);
        
        //Assertions
        Assert.That(resultJSON, Is.EqualTo(expectedJSON));
        Assert.That(resultSections.Count, Is.EqualTo(expectedSections.Count));
        Assert.That(resultSections, Is.Not.SameAs(expectedSections));
        Assert.That(resultSections.Select(section => section.CandidateVotes), 
            Is.Not.SameAs(expectedSections.Select(section => section.CandidateVotes)));
        Assert.That(resultSections.Select(section => section.SectionID), 
            Is.EquivalentTo(expectedSections.Select(section => section.SectionID)));
        Assert.That(resultSections.Select(section => section.CandidateVotes), 
            Is.EquivalentTo(expectedSections.Select(section => section.CandidateVotes)));

    }
    
    [Test]
    [Repeat(5)]
    public async Task RemoveRedundantSectionsAsync_Should_Not_Remove_Sections_If_All_New()
    {
        //Arrange
        SeedData seedData2 = _seedDataBuilder.GenerateNew(10U, 5U);

        //Copy new seedData so we can keep track of the original data after changes
        List<Section> expectedSections = new(seedData2.Sections);

        //Act
        List<Section> resultSections = await _domainService.RemoveRedundantSectionsAsync(expectedSections);
        resultSections.Sort((x, y) => x.SectionID > y.SectionID ? 1 : -1 );
        expectedSections.Sort((x, y) => x.SectionID > y.SectionID ? 1 : -1);
        string resultJSON = JsonSerializer.Serialize(resultSections);
        string expectedJSON = JsonSerializer.Serialize(expectedSections);
        
        //Assertions
        Assert.That(resultJSON, Is.EqualTo(expectedJSON));
        Assert.That(resultSections.Count, Is.EqualTo(seedData2.Sections.Count));
        Assert.That(resultSections.Count, Is.EqualTo(expectedSections.Count));
        Assert.That(resultSections, Is.Not.SameAs(expectedSections));
        Assert.That(resultSections.Select(section => section.CandidateVotes), 
            Is.Not.SameAs(expectedSections.Select(section => section.CandidateVotes)));
        Assert.That(resultSections.Select(section => section.SectionID), 
            Is.EquivalentTo(expectedSections.Select(section => section.SectionID)));
        Assert.That(resultSections.Select(section => section.CandidateVotes), 
            Is.EquivalentTo(expectedSections.Select(section => section.CandidateVotes)));
    }
    
    [Test]
    public async Task RemoveRedundantSectionsAsync_Should_Return_Empty_When_No_New_Sections()
    {
        //Arrange
        //New empty list
        List<Section> expectedSections = new List<Section>();

        //Act
        List<Section> resultSections = await _domainService.RemoveRedundantSectionsAsync(_seedData.Sections);
        string resultJSON = JsonSerializer.Serialize(resultSections);
        string expectedJSON = JsonSerializer.Serialize(expectedSections);
        
        //Assertions
        Assert.That(resultJSON, Is.EqualTo(expectedJSON));
        Assert.That(resultSections, Is.Empty);
        Assert.That(resultSections.Count, Is.EqualTo(expectedSections.Count));
        Assert.That(resultSections.Select(section => section.SectionID), 
            Is.EquivalentTo(expectedSections.Select(section => section.SectionID)));
        Assert.That(resultSections.Select(section => section.CandidateVotes), 
            Is.EquivalentTo(expectedSections.Select(section => section.CandidateVotes)));
    }
    
    [Test]
    public async Task RemoveRedundantSectionsAsync_Should_Return_Empty_When_Sections_Passed_Is_Empty()
    {
        //Arrange
        //New empty list
        List<Section> expectedSections = new();

        //Act
        List<Section> result = await _domainService.RemoveRedundantSectionsAsync(expectedSections);
        string resultJSON = JsonSerializer.Serialize(result);
        string expectedJSON = JsonSerializer.Serialize(expectedSections);
        
        //Assertions
        Assert.That(resultJSON, Is.EqualTo(expectedJSON));
        Assert.That(result, Is.Empty);
        Assert.That(result.Count, Is.EqualTo(expectedSections.Count));
        Assert.That(result.Select(section => section.SectionID), 
            Is.EquivalentTo(expectedSections.Select(section => section.SectionID)));
        Assert.That(result.Select(section => section.CandidateVotes), 
            Is.EquivalentTo(expectedSections.Select(section => section.CandidateVotes)));
    }
}