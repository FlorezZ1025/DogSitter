using UDEM.DEVOPS.DogSitter.Domain.Exceptions;

namespace UDEM.DEVOPS.DogSitter.Domain.Entities;

public class Voter(string nid, DateTime dateOfBirth, string origin) : DomainEntity
{
    const int MINIMUM_AGE = 18;
    const int CHARACTER_QUANTITY = 8;
    const string COUNTRY_OF_ORIGIN = "COLOMBIA";

    public bool IsUnderAge => new DateTime((DateTime.UtcNow - DateOfBirth).Ticks, DateTimeKind.Utc).Year - 1 < MINIMUM_AGE;
    public bool CanVoteBasedOnLocation => string.Equals(Origin, COUNTRY_OF_ORIGIN, StringComparison.InvariantCultureIgnoreCase);
    public string Nid { get; init; } = nid.Length >= CHARACTER_QUANTITY ? nid : throw new CoreBusinessException("the document requires at least 8 chars");
    public DateTime DateOfBirth { get; init; } = dateOfBirth;
    public string Origin { get; init; } = origin;
}

