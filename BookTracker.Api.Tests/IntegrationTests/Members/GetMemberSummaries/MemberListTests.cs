using System.Net;
using BookTracker.Api.Application;
using BookTracker.Api.Application.Members.GetMemberSummaries;
using BookTracker.Api.Domain.Members;

namespace BookTracker.Api.Tests.IntegrationTests.Members.GetMemberSummaries;

public class MemberListTests : IntegrationTest
{
    [Fact]
    public async Task GetMembersReturnsMembers()
    {
        Writer.Seed(db => db.Members.Add(
            new Member
            {
                Name = new MemberName("Ada Lovelace"),
                Email = new MemberEmail("ada@example.com")
            }
        ));

        var response = await Client.GetAsync("/members");

        var result = await response.ReadJsonAs<PagedResult<MemberSummary>>(HttpStatusCode.OK);

        var member = Assert.Single(result.Items);

        Assert.Equal("Ada Lovelace", member.Name);
        Assert.Equal("ada@example.com", member.Email);
        Assert.Equal(1, result.TotalItems);
    }

    [Fact]
    public async Task GetMembersCanSearchByName()
    {
        Writer.Seed(db =>
        {
            db.Members.AddRange(
                new Member
                {
                    Name = new MemberName("Ada Lovelace"),
                    Email = new MemberEmail("ada@example.com")
                },
                new Member
                {
                    Name = new MemberName("Grace Hopper"),
                    Email = new MemberEmail("grace@example.com")
                });
        });

        var response = await Client.GetAsync("/members?search=ada");

        var result = await response.ReadJsonAs<PagedResult<MemberSummary>>(HttpStatusCode.OK);

        var member = Assert.Single(result.Items);

        Assert.Equal("Ada Lovelace", member.Name);
        Assert.Equal(1, result.TotalItems);
    }

    [Fact]
    public async Task GetMembersCanSearchByEmail()
    {
        Writer.Seed(db =>
        {
            db.Members.AddRange(
                new Member
                {
                    Name = new MemberName("Ada Lovelace"),
                    Email = new MemberEmail("ada@example.com")
                },
                new Member
                {
                    Name = new MemberName("Grace Hopper"),
                    Email = new MemberEmail("grace@example.com")
                });
        });

        var response = await Client.GetAsync("/members?search=grace@example.com");

        var result = await response.ReadJsonAs<PagedResult<MemberSummary>>(HttpStatusCode.OK);

        var member = Assert.Single(result.Items);

        Assert.Equal("Grace Hopper", member.Name);
        Assert.Equal(1, result.TotalItems);
    }

    [Fact]
    public async Task GetMembersAppliesPagingAfterSearch()
    {
        Writer.Seed(db =>
        {
            db.Members.AddRange(
                new Member
                {
                    Name = new MemberName("Ada Lovelace"),
                    Email = new MemberEmail("ada1@example.com")
                },
                new Member
                {
                    Name = new MemberName("Ada Byron"),
                    Email = new MemberEmail("ada2@example.com")
                });
        });

        var response = await Client.GetAsync("/members?search=ada&page=2&pageSize=1");

        var result = await response.ReadJsonAs<PagedResult<MemberSummary>>(HttpStatusCode.OK);

        var member = Assert.Single(result.Items);

        Assert.Equal(2, result.Page);
        Assert.Equal(1, result.PageSize);
        Assert.Equal(2, result.TotalItems);
    }
}