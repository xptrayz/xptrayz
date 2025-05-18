using System;
using System.Collections.Generic;

interface IPinjam
{
    void PinjamBuku(Buku buku);
    void KembalikanBuku(Buku buku);
    void LihatBukuDipinjam();
}

abstract class Buku
{
    public string Judul { get; set; }
    public string Penulis { get; set; }
    public int TahunTerbit { get; set; }

    public Buku(string judul, string penulis, int tahun)
    {
        Judul = judul;
        Penulis = penulis;
        TahunTerbit = tahun;
    }

    public abstract void TampilkanInfo();
}

class BukuFiksi : Buku
{
    public BukuFiksi(string judul, string penulis, int tahun) : base(judul, penulis, tahun) { }

    public override void TampilkanInfo()
    {
        Console.WriteLine($"[Fiksi] Judul: {Judul}, Penulis: {Penulis}, Tahun: {TahunTerbit}");
    }
}

class BukuNonFiksi : Buku
{
    public BukuNonFiksi(string judul, string penulis, int tahun) : base(judul, penulis, tahun) { }

    public override void TampilkanInfo()
    {
        Console.WriteLine($"[Non-Fiksi] Judul: {Judul}, Penulis: {Penulis}, Tahun: {TahunTerbit}");
    }
}

class Majalah : Buku
{
    public Majalah(string judul, string penulis, int tahun) : base(judul, penulis, tahun) { }

    public override void TampilkanInfo()
    {
        Console.WriteLine($"[Majalah] Judul: {Judul}, Editor: {Penulis}, Tahun: {TahunTerbit}");
    }
}

class Anggota : IPinjam
{
    private List<Buku> bukuDipinjam = new List<Buku>();
    private const int MaksPinjam = 3;

    public void PinjamBuku(Buku buku)
    {
        if (bukuDipinjam.Count >= MaksPinjam)
            Console.WriteLine("Maksimal peminjaman 3 buku.");
        else
        {
            bukuDipinjam.Add(buku);
            Console.WriteLine($"Berhasil meminjam: {buku.Judul}");
        }
    }

    public void KembalikanBuku(Buku buku)
    {
        if (bukuDipinjam.Remove(buku))
            Console.WriteLine($"Berhasil mengembalikan: {buku.Judul}");
        else
            Console.WriteLine("Buku tidak ditemukan dalam daftar pinjaman.");
    }

    public void LihatBukuDipinjam()
    {
        Console.WriteLine("\nDaftar Buku yang Dipinjam:");
        foreach (var buku in bukuDipinjam)
            buku.TampilkanInfo();
        if (bukuDipinjam.Count == 0) Console.WriteLine("Belum ada buku dipinjam.");
    }
}

class Program
{
    static List<Buku> daftarBuku = new List<Buku>();
    static Anggota user = new Anggota();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== PERPUSTAKAAN MINI ===");
            Console.WriteLine("1. Tambah Buku");
            Console.WriteLine("2. Lihat Daftar Buku");
            Console.WriteLine("3. Ubah Buku");
            Console.WriteLine("4. Pinjam Buku");
            Console.WriteLine("5. Kembalikan Buku");
            Console.WriteLine("6. Lihat Buku Dipinjam");
            Console.WriteLine("0. Keluar");
            Console.Write("Pilih: ");
            string pilih = Console.ReadLine();

            switch (pilih)
            {
                case "1": TambahBuku(); break;
                case "2": TampilkanSemuaBuku(); break;
                case "3": UbahDataBuku(); break;
                case "4": PinjamBuku(); break;
                case "5": KembalikanBuku(); break;
                case "6": user.LihatBukuDipinjam(); break;
                case "0": return;
                default: Console.WriteLine("Pilihan tidak valid."); break;
            }
        }
    }

    static void TambahBuku()
    {
        Console.WriteLine("\nJenis Buku: 1. Fiksi  2. Non-Fiksi  3. Majalah");
        string jenis = Console.ReadLine();

        Console.Write("Judul: "); string judul = Console.ReadLine();
        Console.Write("Penulis/Editor: "); string penulis = Console.ReadLine();
        Console.Write("Tahun Terbit: "); int tahun = int.Parse(Console.ReadLine());

        Buku buku = jenis switch
        {
            "1" => new BukuFiksi(judul, penulis, tahun),
            "2" => new BukuNonFiksi(judul, penulis, tahun),
            "3" => new Majalah(judul, penulis, tahun),
            _ => null
        };

        if (buku != null)
        {
            daftarBuku.Add(buku);
            Console.WriteLine("Buku berhasil ditambahkan!");
        }
        else
            Console.WriteLine("Jenis buku tidak valid.");
    }

    static void TampilkanSemuaBuku()
    {
        Console.WriteLine("\nDaftar Buku:");
        if (daftarBuku.Count == 0)
        {
            Console.WriteLine("Belum ada buku.");
            return;
        }

        int i = 1;
        foreach (var buku in daftarBuku)
        {
            Console.Write($"{i++}. ");
            buku.TampilkanInfo();
        }
    }

    static void UbahDataBuku()
    {
        TampilkanSemuaBuku();
        Console.Write("Pilih nomor buku yang ingin diubah: ");
        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= daftarBuku.Count)
        {
            Buku buku = daftarBuku[idx - 1];
            Console.Write("Judul baru: "); buku.Judul = Console.ReadLine();
            Console.Write("Penulis/Editor baru: "); buku.Penulis = Console.ReadLine();
            Console.Write("Tahun Terbit baru: "); buku.TahunTerbit = int.Parse(Console.ReadLine());
            Console.WriteLine("Buku berhasil diubah.");
        }
        else
        {
            Console.WriteLine("Nomor tidak valid.");
        }
    }

    static void PinjamBuku()
    {
        TampilkanSemuaBuku();
        Console.Write("Pilih nomor buku untuk dipinjam: ");
        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= daftarBuku.Count)
        {
            user.PinjamBuku(daftarBuku[idx - 1]);
        }
        else
        {
            Console.WriteLine("Nomor tidak valid.");
        }
    }

    static void KembalikanBuku()
    {
        Console.Write("Masukkan judul buku yang dikembalikan: ");
        string judul = Console.ReadLine();
        Buku buku = daftarBuku.Find(b => b.Judul.Equals(judul, StringComparison.OrdinalIgnoreCase));
        if (buku != null)
            user.KembalikanBuku(buku);
        else
            Console.WriteLine("Buku tidak ditemukan.");
    }
}
