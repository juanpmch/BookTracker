using System.Net;
using System.Net.Http.Json;
using BookTracker.Api.Application.UpdateMember;
using BookTracker.Api.Domain.Members;

namespace BookTracker.Api.Tests.IntegrationTests.Members.UpdateMember;

public class UpdateMemberTests : IntegrationTest
{
    [Fact]
    public async Task PutMemberUpdatesMember()
    {
        Writer.Seed(db => db.Members.Add(
            new Member
            {
                Name = new MemberName("Ada Lovelace"),
                Email = new MemberEmail("ada@example.com")
            }
        ));

        var request =
            new UpdateMemberRequest
            {
                Name = "Ada Byron",
                Email = "ada.byron@example.com"
            };

        var response = await Client.PutAsJsonAsync("/members/1", request);

        await response.ShouldHaveStatusCode(HttpStatusCode.NoContent);

        var member = Reader.Query(db => db.Members.Find(1));

        Assert.NotNull(member);
        Assert.Equal("Ada Byron", member.Name.Value);
        Assert.Equal("ada.byron@example.com", member.Email.Value);
    }

    [Fact]
    public async Task PutMemberReturnsNotFoundWhenMemberDoesNotExist()
    {
        var request =
            new UpdateMemberRequest
            {
                Name = "Unknown",
                Email = "unknown@example.com"
            };

        var response = await Client.PutAsJsonAsync("/members/9999", request);

        await response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutMemberReturnsBadRequestWhenEmailIsInvalid()
    {
        Writer.Seed(db => db.Members.Add(
            new Member
            {
                Name = new MemberName("Ada Lovelace"),
                Email = new MemberEmail("ada@example.com")
            }
        ));

        var request =
            new UpdateMemberRequest
            {
                Name = "Ada Lovelace",
                Email = "not-an-email"
            };

        var response = await Client.PutAsJsonAsync("/members/1", request);

        await response.ShouldHaveStatusCode(HttpStatusCode.BadRequest);
    }
}