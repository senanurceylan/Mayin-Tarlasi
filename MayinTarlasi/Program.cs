using System;

namespace MayinTarlasi
{
    class Program
    {
        static int boyut = 20; // Oyun alanının boyutunu belirtiyoruz (20x20).
        static char[,] oyunAlani = new char[boyut, boyut]; // Oyun alanını tutan bir matris oluşturuyoruz.
        static bool[,] mayinlar = new bool[boyut, boyut]; // Mayınların nerede olduğunu tutmak için bir matris.
        static Random rastgele = new Random(); // Mayınların rastgele yerleştirilmesi için bir Random nesnesi oluşturuyoruz.

        static void Main(string[] args)
        {
            MayinlariOlustur(); // Mayınları rastgele olarak oyun alanına yerleştiriyoruz.
            OyunAlaniniOlustur(); // Oyun alanını başlangıçta kapalı (-) olarak oluşturuyoruz.
            OyunBaslat(); // Oyunu başlatıyoruz.
        }

        static void MayinlariOlustur()
        {
            int toplamMayin = boyut * boyut / 5; // Toplam hücrelerin %20'sine mayın yerleştiriyoruz.
            for (int i = 0; i < toplamMayin; i++)
            {
                int x, y;
                do
                {
                    x = rastgele.Next(0, boyut);
                    y = rastgele.Next(0, boyut);
                } while (mayinlar[x, y]); // Eğer bu hücrede zaten mayın varsa başka bir hücre seçiyoruz.
                mayinlar[x, y] = true; // Mayını bu hücreye yerleştiriyoruz.
            }
        }

        static void OyunAlaniniOlustur()
        {
            for (int i = 0; i < boyut; i++)
            {
                for (int j = 0; j < boyut; j++)
                {
                    oyunAlani[i, j] = '-'; // Tüm hücreleri başlangıçta kapalı (-) olarak işaretliyoruz.
                }
            }
        }

        static void OyunBaslat()
        {
            bool oyunDevam = true; // Oyun devam ettiği sürece bu değişken true kalacak.

            while (oyunDevam)
            {
                Console.Clear(); // Ekranı temizliyoruz.
                OyunAlaniniYazdir(); // Mevcut oyun alanını ekrana yazdırıyoruz.

                Console.WriteLine("hocam hoşgeldinizzzz umarım beğenirsiniz":));

                Console.Write("Bir hücre seç (örnek: 5,5): ");
                string input = Console.ReadLine();

                // Geçersiz giriş kontrolü
                if (string.IsNullOrWhiteSpace(input) || !input.Contains(","))
                {
                    Console.WriteLine("Geçersiz giriş! Lütfen 'x,y' formatında bir koordinat gir");
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }

                string[] girdi = input.Split(',');

                if (girdi.Length != 2 || !int.TryParse(girdi[0], out int x) || !int.TryParse(girdi[1], out int y))
                {
                    Console.WriteLine("Geçersiz giriş! Lütfen 'x,y' formatında bir koordinat gir");
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }

                if (x >= 0 && x < boyut && y >= 0 && y < boyut) // Geçerli koordinatlar kontrolü
                {
                    if (mayinlar[x, y]) // Eğer seçilen hücrede mayın varsa
                    {
                        Console.Clear();
                        Console.WriteLine("Mayına bastın! Oyun bitti.");
                        OyunAlaniniYazdir(true); // Tüm mayınları gösteriyoruz.
                        oyunDevam = false;
                        Console.WriteLine("Oyunu bitirmek için bir tuşa basınzı");
                        Console.ReadLine();
                    }
                    else
                    {
                        // Seçilen hücrede mayın yoksa, otomatik hücre açmayı kontrol ediyoruz
                        oyunAlani[x, y] = MayinSayisiniHesapla(x, y);
                        if (oyunAlani[x, y] == '0')
                        {
                            OtomatikHucreselAcma(x, y);
                        }

                        if (KazandiMi())
                        {
                            Console.Clear();
                            Console.WriteLine("Tüm mayınlardan kaçmayı başardın");
                            OyunAlaniniYazdir(true);
                            oyunDevam = false;
                            Console.WriteLine("Oyunu bitirmek için bir tuşa basın");
                            Console.ReadLine();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz koordinat girdin Lütfen tekrar dene.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        static char MayinSayisiniHesapla(int x, int y)
        {
            int sayac = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int yeniX = x + i;
                    int yeniY = y + j;
                    if (yeniX >= 0 && yeniX < boyut && yeniY >= 0 && yeniY < boyut && mayinlar[yeniX, yeniY])
                    {
                        sayac++;
                    }
                }
            }
            return sayac.ToString()[0]; // Mayın sayısını karakter olarak döndürüyoruz.
        }

        static void OtomatikHucreselAcma(int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int yeniX = x + i;
                    int yeniY = y + j;
                    if (yeniX >= 0 && yeniX < boyut && yeniY >= 0 && yeniY < boyut && oyunAlani[yeniX, yeniY] == '-')
                    {
                        oyunAlani[yeniX, yeniY] = MayinSayisiniHesapla(yeniX, yeniY);
                        if (oyunAlani[yeniX, yeniY] == '0') // Eğer etrafında mayın yoksa zincirleme açma
                        {
                            OtomatikHucreselAcma(yeniX, yeniY);
                        }
                    }
                }
            }
        }

        static void OyunAlaniniYazdir(bool mayinlariGoster = false)
        {
            for (int i = 0; i < boyut; i++)
            {
                for (int j = 0; j < boyut; j++)
                {
                    if (mayinlariGoster && mayinlar[i, j])
                    {
                        Console.Write('*'); // Mayın olan hücreleri yıldız (*) ile gösteriyoruz.
                    }
                    else
                    {
                        Console.Write(oyunAlani[i, j]);
                    }
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        static bool KazandiMi()
        {
            for (int i = 0; i < boyut; i++)
            {
                for (int j = 0; j < boyut; j++)
                {
                    if (!mayinlar[i, j] && oyunAlani[i, j] == '-')
                    {
                        return false; // Hâlâ açılmamış güvenli hücre varsa
                    }
                }
            }
            return true;
        }
    }
}