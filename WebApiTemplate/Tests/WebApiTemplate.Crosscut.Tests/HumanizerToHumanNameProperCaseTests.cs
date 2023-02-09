using WebApiTemplate.Crosscut.Extensions;

namespace WebApiTemplate.Crosscut.Tests;

public class HumanizerToHumanNameProperCaseTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("Anrijs", "Anrijs")]
    [InlineData("ANrijs", "Anrijs")]
    [InlineData("anrijs", "Anrijs")]
    [InlineData("aNrijs", "Anrijs")]
    [InlineData("Anrijs Vītoliņš", "Anrijs Vītoliņš")]
    [InlineData("ANRIJS VĪTOLIŅŠ", "Anrijs Vītoliņš")]
    [InlineData("anrijs vītoliņš", "Anrijs Vītoliņš")]
    [InlineData("ĶĒNIŅŠ", "Ķēniņš")]
    [InlineData("macleod", "MacLeod")]
    [InlineData("MCDONALDS", "McDonalds")]
    [InlineData("Paul McCartney", "Paul McCartney")]
    [InlineData("paul mccartney", "Paul McCartney")]
    [InlineData("PAUL MCCARTNEY", "Paul McCartney")]
    [InlineData("vanBrooks", "VanBrooks")]
    [InlineData("LitlCoolMan", "LitlCoolMan")]
    [InlineData("d'angelo", "D'Angelo")]
    [InlineData("tommy LEE carter", "Tommy LEE Carter")]
    [InlineData("mary-jane", "Mary-Jane")]
    [InlineData("JOËL VON WINTEREGG", "Joël von Winteregg")]
    [InlineData("jose de la acosta", "Jose de la Acosta")]
    [InlineData("VAN DER SLOOT", "van der Sloot")]
    [InlineData("HYACINT De BUCKET", "Hyacint de Bucket")]
    public void ToProperCase_Values_AreTransformed(string testable, string expected)
    => testable.ToHumanNameProperCase().Should().Be(expected);
}
