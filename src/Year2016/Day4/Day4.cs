using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Year2016;

public partial class Day4
{
    [Theory]
    [FileLines("data_sample.txt", 1514)]
    [FileLines("data.txt", 158835)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var sectorSum = data
            .Select(ParseRoom)
            .Where(r => r.Checksum == r.ExpectedChecksum())
            .Sum(r => r.SectorId)
            ;

        Assert.Equal(result, sectorSum);
    }

    [Theory]
    [FileLines("data.txt", 993)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var sectorId = data
            .Select(ParseRoom)
            .Where(r => r.Checksum == r.ExpectedChecksum())
            .Select(r => (sectorId: r.SectorId, decryptedName: r.DecryptName()))
            .Single(r => r.decryptedName == "northpole object storage")
            .sectorId;

        Assert.Equal(result, sectorId);
    }

    [GeneratedRegex(@"([a-z-]+)-([0-9]+)\[([a-z]+)\]")]
    private partial Regex RoomParts();

    private RoomId ParseRoom(string rawId)
    {
        var regex = RoomParts();
        var parts = regex.Matches(rawId)[0].Groups.Values.ToArray()[1..];
        var name = parts[0].Value;
        var sector = int.Parse(parts[1].Value);
        var checksum = parts[2].Value;

        return new RoomId(name, sector, checksum);
    }

    private record RoomId(string EncryptedName, int SectorId, string Checksum)
    {
        public string ExpectedChecksum() => EncryptedName
            .GroupBy(_ => _)
            .Where(t => t.Key != '-')
            .Select(t => (letter: t.Key, count: t.Count()))
            .OrderBy(t => t.letter)
            .OrderByDescending(t => t.count)
            .Take(5)
            .Select(t => t.letter)
            .ToArray()
            .AsSpan()
            .AsString()
            ;

        public string DecryptName()
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            var name = EncryptedName.ToCharArray();
            for (var i = 0; i < name.Length; i++)
            {
                name[i] = name[i] switch
                {
                    '-' => ' ',
                    char c => alphabet[(alphabet.IndexOf(c) + SectorId) % alphabet.Length]
                };
            }

            return new(name);
        }
    }
}
