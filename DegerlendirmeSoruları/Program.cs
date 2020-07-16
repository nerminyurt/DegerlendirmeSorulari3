namespace DegerlendirmeSoruları
{
    using DegerlendirmeSoruları.CustomObjects;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Metadata;

    class Program
    {
        static void Main(string[] args)
        {
            int musteriNumarasi = 15000000;

            CalistirmaMotoru.KomutCalistir("MuhasebeModülü", "MaaşYatır", new object[] { musteriNumarasi });

            CalistirmaMotoru.KomutCalistir("MuhasebeModülü", "YıllıkÜcretTahsilEt", new object[] { musteriNumarasi });

            CalistirmaMotoru.BekleyenIslemleriGerceklestir("MuhasebeModülü", musteriNumarasi);
        }
    }

    public class CalistirmaMotoru
    {
        public static object[] KomutCalistir(string modulSinifAdi, string methodAdi, object[] inputs)
        {
            try
            {
                if (checkCustomerNumber(inputs))
                {
                    Type typeOfClass = Type.GetType("DegerlendirmeSoruları." + modulSinifAdi + ", DegerlendirmeSoruları");
                    typeOfClass.GetMethod(methodAdi, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Activator.CreateInstance(typeOfClass), inputs);
                    return null;    //burada ne return edeceğimden emin olamadım böyle kaldı.
                }
                else
                {
                    throw new Exception("Müşteri Numarası valid değil!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
                throw ex;
            }
        }

        private static bool checkCustomerNumber(object[] inputs)
        {
            int result = 0;
            return int.TryParse(inputs[0].ToString(), out result);  //returns true if input[0] is a valid integer.
        }

        public static void BekleyenIslemleriGerceklestir(string modulSinifAdi, int musteriNumarasi)
        {
            try
            {
                VeritabaniIslemleri veriTabaniIslemi = new VeritabaniIslemleri();
                List<MethodDefinitions> methodDefinitions = veriTabaniIslemi.getMethodDefinitions();
                foreach (var method in methodDefinitions)
                {
                    if (method.isPostponedJob)
                        KomutÇalıştır(modulSinifAdi, method.methodName, new object[] { musteriNumarasi });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  
                throw ex;
            }
        }
    }

    public class MuhasebeModulu
    {
        private void MaasYatir(int musteriNumarasi)
        {
            // gerekli işlemler gerçekleştirilir.
            Console.WriteLine(string.Format("{0} numaralı müşterinin maaşı yatırıldı.", musteriNumarasi));
        }

        private void YillikUcretTahsilEt(int musteriNumarasi)
        {
            // gerekli işlemler gerçekleştirilir.
            Console.WriteLine("{0} numaralı müşteriden yıllık kart ücreti tahsil edildi.", musteriNumarasi);
        }

        private void OtomatikOdemeleriGerceklestir(int musteriNumarasi)
        {
            // gerekli işlemler gerçekleştirilir.
            Console.WriteLine("{0} numaralı müşterinin otomatik ödemeleri gerçekleştirildi.", musteriNumarasi);
        }
    }

    public class VeritabaniIslemleri
    {
        public List<MethodDefinitions> getMethodDefinitions()
        {
            List<MethodDefinitions> methodDefinitions = new List<MethodDefinitions>();
            methodDefinitions.Add(new MethodDefinitions { isPostponedJob = false, methodName = "MaaşYatır" });
            methodDefinitions.Add(new MethodDefinitions { isPostponedJob = false, methodName = "YıllıkÜcretTahsilEt" });
            methodDefinitions.Add(new MethodDefinitions { isPostponedJob = true, methodName = "OtomatikÖdemeleriGerçekleştir" });
            return methodDefinitions;
        }
    }
}
