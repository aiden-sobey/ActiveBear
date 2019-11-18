using ActiveBear.Services;
using NUnit.Framework;

namespace ActiveBear.Spec.Services
{
    [TestFixture]
    public class EncryptionServiceSpec
    {
        internal const string Lorem = "Lorem";

        [Test]
        public void ShaOfEmptyStringIsEmpty()
        {
            var sha = EncryptionService.Sha256(string.Empty);
            Assert.IsTrue(string.IsNullOrEmpty(sha));

            sha = EncryptionService.Sha256(null);
            Assert.IsTrue(string.IsNullOrEmpty(sha));
        }

        [Test]
        public void ShaOfStringAltersContents()
        {
            Assert.AreNotEqual(Lorem, EncryptionService.Sha256(Lorem));
        }

        [Test]
        public void ShaOutputIsConsistent()
        {
            var shaOne = EncryptionService.Sha256(Lorem);
            var shaTwo = EncryptionService.Sha256(Lorem);

            Assert.AreEqual(shaOne, shaTwo);
        }

        [Test]
        public void ShaOutputIsNotDistributive()
        {
            Assert.AreNotEqual(
                EncryptionService.Sha256(Lorem + Lorem),
                EncryptionService.Sha256(Lorem) + EncryptionService.Sha256(Lorem)
            );
        }

        [Test]
        public void ShaHasNoTrivialCollisions()
        {
            var lore = EncryptionService.Sha256("Lore");
            var lorem = EncryptionService.Sha256("Lorem");
            var lorema = EncryptionService.Sha256("Lorema");

            Assert.AreNotEqual(lore, lorem);
            Assert.AreNotEqual(lorem, lorema);
            Assert.AreNotEqual(lore, lorema);
        }
    }
}
