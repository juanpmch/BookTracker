using BookTracker.Api.Domain;
using BookTracker.Api.Domain.Members;

namespace BookTracker.Api.Tests.Domain.Members;

public class MemberNameTests
{
    [Fact]
    public void Constructor_WithValidName_ShouldCreateMemberName()
    {
        var memberName = new MemberName("Juan");

        Assert.Equal("Juan", memberName.Value);
    }

    [Fact]
    public void Constructor_ShouldTrimWhitespace()
    {
        var memberName = new MemberName("   Juan   ");

        Assert.Equal("Juan", memberName.Value);
    }

    [Fact]
    public void Constructor_WithWhitespace_ShouldThrowDomainException()
    {
        Assert.Throws<DomainException>(() => new MemberName("     "));
    }

    [Fact]
    public void Constructor_WithTooLongName_ShouldThrowDomainException()
    {
        var name = new string('A', 101);

        Assert.Throws<DomainException>(() => new MemberName(name));
    }

    [Fact]
public void Constructor_WithNull_ShouldThrowDomainException()
{
    var exception = Assert.Throws<DomainException>(() => new MemberName(null!));

    Assert.Equal("Name is required.", exception.Message);
}
}