using System.Net;
using System.Net.Http.Json;
using BookTracker.Api.Application.CreateMember;
using BookTracker.Api.Domain.Members;

namespace BookTracker.Api.Tests.IntegrationTests.Members.CreateMember;

public class CreateMemberTests : IntegrationTest
{
    [Fact]
    public async Task PostMemberCreatesMember()
    {
        var request =
            new CreateMemberRequest
            {
                Name = "Ada Lovelace",
                Email = "ada@example.com"
            };

        var response = await Client.PostAsJsonAsync("/members", request);

        var created = await response.ReadJsonAs<CreateMemberResponse>(HttpStatusCode.Created);

        Assert.True(created.Id > 0);
        Assert.Equal("Ada Lovelace", created.Name);
        Assert.Equal("ada@example.com", created.Email);

        var member = Reader.Query(context => context.Find<Member>(created.Id));

        Assert.NotNull(member);
        Assert.Equal("Ada Lovelace", member.Name.Value);
        Assert.Equal("ada@example.com", member.Email.Value);
    }

    [Fact]
    public async Task PostMemberReturnsBadRequestWhenNameIsWhitespace()
    {
        var request =
            new CreateMemberRequest
            {
                Name = "   ",
                Email = "ada@example.com"
            };

        var response = await Client.PostAsJsonAsync("/members", request);

        await response.ShouldHaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostMemberReturnsBadRequestWhenEmailIsInvalid()
    {
        var request =
            new CreateMemberRequest
            {
                Name = "Ada Lovelace",
                Email = "not-an-email"
            };

        var response = await Client.PostAsJsonAsync("/members", request);

        await response.ShouldHaveStatusCode(HttpStatusCode.BadRequest);
    }
}