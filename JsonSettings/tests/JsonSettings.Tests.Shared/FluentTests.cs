using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucs.JsonSettings;
using Nucs.JsonSettings.Fluent;


namespace Nucs.JsonSettings.Tests {
    [TestClass]
    public class FluentTests {

        [TestMethod]
        public void Fluent_WithFileNameAndEncryptionAndAutosave() {
            using (var f = new TempfileLife()) {
                Func<SettingsBag> gen = () => new SettingsBag().WithFileName((string) f).WithEncryption("qweqwe").LoadNow().EnableAutosave();
                var o = gen();
                o["lol"] = "xoxo";
                o["loly"] = 2;
                var x = gen();
                x["lol"].ShouldBeEquivalentTo("xoxo");
                x["loly"].ShouldBeEquivalentTo(2);
            }
        }

        [TestMethod]
        public void Fluent_WithBas64() {
            using (var f = new TempfileLife()) {
                Func<SettingsBag> gen = () => new SettingsBag().WithFileName((string) f).WithBase64().LoadNow().EnableAutosave();
                var o = gen();
                o["lol"] = "xoxo";
                o["loly"] = 2;
                var x = gen();
                x["lol"].ShouldBeEquivalentTo("xoxo");
                x["loly"].ShouldBeEquivalentTo(2);
            }
        }

        [TestMethod]
        public void Fluent_WithEncryptionAndWithBas64() {
            using (var f = new TempfileLife()) {
                Func<SettingsBag> gen = () => new SettingsBag().WithFileName((string) f).WithEncryption("qweqwe").WithBase64().LoadNow().EnableAutosave();
                var o = gen();
                o["lol"] = "xoxo";
                o["loly"] = 2;
                var x = gen();
                x["lol"].ShouldBeEquivalentTo("xoxo");
                x["loly"].ShouldBeEquivalentTo(2);
            }
        }

        [TestMethod]
        public void Fluent_WithhBas64AndEncryption() {
            using (var f = new TempfileLife()) {
                Func<SettingsBag> gen = () => new SettingsBag().WithFileName((string) f).WithBase64().WithEncryption("qweqwe").LoadNow().EnableAutosave();
                var o = gen();
                o["lol"] = "xoxo";
                o["loly"] = 2;
                var x = gen();
                x["lol"].ShouldBeEquivalentTo("xoxo");
                x["loly"].ShouldBeEquivalentTo(2);
            }
        }

        [TestMethod]
        public void Fluent_SimpleLoad() {
            using (var f = new TempfileLife()) {
                Func<SettingsBag> gen = () => new SettingsBag().WithFileName((string) f).LoadNow().EnableAutosave();
                var o = gen();
                o["lol"] = "xoxo";
                o["loly"] = 2;
                var x = gen();
                x["lol"].ShouldBeEquivalentTo("xoxo");
                x["loly"].ShouldBeEquivalentTo(2);
            }
        }

        [TestMethod]
        public void Fluent_SimpleSave() {
            using (var f = new TempfileLife()) {
                Func<SettingsBag> gen = () => new SettingsBag().WithFileName((string) f).LoadNow();
                var o = gen();
                o["lol"] = "xoxo";
                o["loly"] = 2;
                o.Save();
            }
        }

        [TestMethod]
        public void Fluent_SavingWithBase64_LoadingWithout() {
            using (var f = new TempfileLife()) {
                //used for autodelete file after test ends
                var o = JsonSettings.Configure<CasualExampleSettings>(f.FileName)
                                    .WithBase64()
                                    .LoadNow();
                o.SomeNumeralProperty = 1;
                o.SomeProperty = "with some value";
                o.SomeClassProperty = new SmallClass() {Name = "Small", Value = "Class"};
                o.Save();

                //validate
                new Action(() => { o = JsonSettings.Configure<CasualExampleSettings>(f.FileName).LoadNow(); })
                   .ShouldThrow<JsonSettingsException>();
            }
        }

        [TestMethod]
        public void Fluent_ConstructorFileNameComparison() {
            using (var f = new TempfileLife()) {
                var o = JsonSettings.Configure<CasualExampleSettings>(f.FileName).WithBase64().WithEncryption("SuperPassword").LoadNow();

                //validate
                o.FileName.Should().EndWith(f.FileName).And.Contain("\\");
                Console.WriteLine($"{f.FileName} -> {o.FileName}");
            }
        }
        [TestMethod]
        public void Fluent_ConstructorFileNameVsWithFilenameComparison() {
            using (var f = new TempfileLife()) {
                var o = JsonSettings.Configure<CasualExampleSettings>(f.FileName).WithBase64().WithEncryption("SuperPassword").LoadNow();
                var n = JsonSettings.Configure<CasualExampleSettings>().WithFileName(f.FileName).WithBase64().WithEncryption("SuperPassword").LoadNow();

                //validate
                o.FileName.Should().Be(n.FileName);
                Console.WriteLine($"{o.FileName} <-> {n.FileName}");
            }
        }
        [TestMethod]
        public void Fluent_PostSaveFilenameComparison() {
            using (var f = new TempfileLife()) {
                var o = JsonSettings.Configure<CasualExampleSettings>(f.FileName).WithBase64().WithEncryption("SuperPassword").LoadNow();
                var n = JsonSettings.Configure<CasualExampleSettings>().WithFileName(f.FileName).WithBase64().WithEncryption("SuperPassword").LoadNow();
                //validate
                o.FileName.Should().Be(n.FileName);
                o.Save();
                o.FileName.Should().Be(n.FileName);
                n.Save();
                o.FileName.Should().Be(n.FileName);

                o.FileName.Should().EndWith(f.FileName);
                n.FileName.Should().EndWith(f.FileName);
                Console.WriteLine($"{o.FileName} <-> {n.FileName}");
            }
        }
        [TestMethod]
        public void Fluent_WithFileName() {
            using (var f = new TempfileLife()) {
                var o = JsonSettings.Configure<CasualExampleSettings>().WithFileName(f.FileName).WithBase64().WithEncryption("SuperPassword").LoadNow();

                //validate
                o.FileName.Should().EndWith(f.FileName);
                Console.WriteLine($"{f.FileName} -> {o.FileName}");
            }
        }

        [TestMethod]
        public void JsonSettings_FileNameIsNullByDefault() {
            new Action(() => { JsonSettings.Load<FilenamelessSettings>(); }).ShouldThrow<JsonSettingsException>();
        }

        [TestMethod]
        public void Fluent_ConstructLoadNow_Issue1_WithLocalPath() { //Issue #1 on github
            using (var f = new TempfileLife()) {
                //validate
                Action act = () => JsonSettings.Construct<SettingsBag>(f.FileName).LoadNow().EnableAutosave();
                act.ShouldNotThrow("LoadNow handles non existent folders and files.");
            }
        }
        [TestMethod]
        public void Fluent_ConstructLoadNow_Issue1_WithRemotePath() { //Issue #1 on github
            using (var f = new TempfileLife( @"\MoalemYar\"+Path.GetRandomFileName())) {
                //validate
                Action act = () => JsonSettings.Construct<SettingsBag>(f.FileName).LoadNow().EnableAutosave();
                act.ShouldNotThrow("LoadNow handles non existent folders and files.");
            }
        }
        class FilterFileNameSettings : JsonSettings {
            public override string FileName { get; set; }
            public FilterFileNameSettings() { }
            public FilterFileNameSettings(string fileName) : base(fileName) { }
        }

        class FilenamelessSettings : JsonSettings {
            public override string FileName { get; set; } = null;
            public string someprop { get; set; }

            public FilenamelessSettings() { }
            public FilenamelessSettings(string fileName) : base(fileName) { }
        }



        public class MySettings : JsonSettings {
            public override string FileName { get; set; } = "TheDefaultFilename"; //for loading and saving.

            #region Settings

            public string SomeProperty { get; set; }
            public int SomeNumberWithDefaultValue { get; set; } = 1;

            #endregion

            public MySettings() { }
            public MySettings(string fileName) : base(fileName) { }
        }
    }
}