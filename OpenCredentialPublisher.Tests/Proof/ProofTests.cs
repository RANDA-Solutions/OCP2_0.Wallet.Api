using System.IO;
using System.Threading.Tasks;
using OpenCredentialPublisher.Proof;

namespace OpenCredentialPublisher.Tests.Proof;

public class ProofTests
{
    private readonly string _clrTestJsonValid;
    private readonly string _clrTestJsonTampered;

    public ProofTests()
    {
        using var streamValid = new StreamReader(typeof(ProofTests).Assembly.GetManifestResourceStream($"{typeof(ProofTests).Namespace}.Files.clr2-valid.json")!);
        _clrTestJsonValid = streamValid.ReadToEnd();

        using var streamTampered = new StreamReader(typeof(ProofTests).Assembly.GetManifestResourceStream($"{typeof(ProofTests).Namespace}.Files.clr2-tampered.json")!);
        _clrTestJsonTampered = streamTampered.ReadToEnd();

    }

    [Fact]
    public async Task VerifyProof_ShouldReturnTrue_WhenClrJsonIsValid()
    {
        var proofService = new ProofService();
        var isVerified = await proofService.VerifyProof(_clrTestJsonValid);
        Assert.True(isVerified);
    }


    [Fact]
    public async Task VerifyProof_ShouldReturnFalse_WhenClrJsonHasBeenTampered()
    {
        var proofService = new ProofService();
        var isVerified = await proofService.VerifyProof(_clrTestJsonTampered);
        Assert.False(isVerified);
    }
}