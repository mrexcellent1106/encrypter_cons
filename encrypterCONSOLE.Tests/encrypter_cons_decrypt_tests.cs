using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using encrypter_console;

namespace encrypter_console.Tests
{
    [TestClass]
    public class Encrypter_cons_decrypt_tests
    {
        [TestMethod]
        public void Caesar_Decrypt_Test()
        {
            //arrange
            int key = 5;
            string res = "Фхнжйч! Вчу чйцч энщхужюнпе! Чшч ьнцру 123, the end.";
            string expected = "Привет! Это тест шифровщика! Тут число 123, the end.";
            //act
            Program.CaesarDecrypt_console(key, res, out string result);
       
            //assert
            Assert.AreEqual(expected, result);
        }      
    }
}
