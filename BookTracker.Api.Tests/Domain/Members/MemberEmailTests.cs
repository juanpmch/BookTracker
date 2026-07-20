using BookTracker.Api.Domain;
using BookTracker.Api.Domain.Members;

namespace BookTracker.Api.Tests.Domain.Members;

public class MemberEmailTests
{
    [Fact]
    public void Constructor_WithValidEmail_ShouldCreateMemberEmail()
    {
        var email = new MemberEmail("juan@test.com");

        Assert.Equal("juan@test.com", email.Value);
    }

    [Fact]
    public void Constructor_ShouldTrimWhitespace()
    {
        var email = new MemberEmail("  juan@test.com  ");

        Assert.Equal("juan@test.com", email.Value);
    }

    [Fact]
    public void Constructor_WithWhitespace_ShouldThrowDomainException()
    {
        Assert.Throws<DomainException>(() => new MemberEmail("   "));
    }

    [Fact]
    public void Constructor_WithTooLongEmail_ShouldThrowDomainException()
    {
        var email = new string('a', 201) + "@test.com";

        Assert.Throws<DomainException>(() => new MemberEmail(email));
    }

    [Fact]
    public void Constructor_WithoutAtSymbol_ShouldThrowDomainException()
    {
        Assert.Throws<DomainException>(() => new MemberEmail("juantest.com"));
    }

    [Fact]
public void Constructor_WithNull_ShouldThrowDomainException()
{
    var exception = Assert.Throws<DomainException>(() => new MemberEmail(null!));

    Assert.Equal("Email is required.", exception.Message);
}
}