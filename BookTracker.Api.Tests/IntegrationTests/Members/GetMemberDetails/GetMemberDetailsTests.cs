using System.Net;
using BookTracker.Api.Application.Members.GetMemberDetails;
using BookTracker.Api.Domain.Members;

namespace BookTracker.Api.Tests.IntegrationTests.Members.GetMemberDetails;

public class GetMemberDetailsTests : IntegrationTest
{
    [Fact]
    public async Task GetMemberByIdReturnsMember()
    {
        Writer.Seed(db => db.Members.Add(
            new Member
            {
                Name = new MemberName("Ada Lovelace"),
                Email = new MemberEmail("ada@example.com")
            }
        ));

        var response = await Client.GetAsync("/members/1");

        var member = await response.ReadJsonAs<GetMemberDetailsResponse>(HttpStatusCode.OK);

        Assert.Equal(1, member.Id);
        Assert.Equal("Ada Lovelace", member.Name);
        Assert.Equal("ada@example.com", member.Email);
    }

    [Fact]
    public async Task GetMemberByIdReturnsNotFoundWhenMemberDoesNotExist()
    {
        var response = await Client.GetAsync("/members/9999");

        await response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }
}
