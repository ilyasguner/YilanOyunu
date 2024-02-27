using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YılanOyunu
{
    public partial class Form1 : Form
    {
        Label _yilaninkafasi;
        int _yilanparçasıarasıboşluk = 2;
        int _yılanparçasısayısı;
        int _yılanparçasıboyutu = 20;
        int _yemboyutu = 20;
        Label _yem;
        Random _random;
        HareketYönü _yön;

        public Form1()
        {
            InitializeComponent();
            _random = new Random();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            _yılanparçasısayısı = 0;
            YemOluştur();
            YeminYeriniDeğiştir();
            YılanıYerleştir();
            timer1.Enabled = true;
        }
        private void YenidenBaşlat()//methot her çalıştığında oyun başa sarar
        {
            this.pnl.Controls.Clear();
            _yılanparçasısayısı = 0;
            YemOluştur();
            YeminYeriniDeğiştir();
            YılanıYerleştir();
            timer1.Enabled = true;
        }
        private Label YılanParçasıOluştur(int locationx, int locationy)//dışarıdan x ve y koordinatlarını parametre olarak alan ve label döndüren yılanın ilk parçasını oluşturan method
        {
            _yılanparçasısayısı++;
            Label label = new Label()
            {
                Name = "yilanParça" + _yılanparçasısayısı,
                BackColor = Color.Blue,
                Width = _yılanparçasıboyutu,
                Height = _yılanparçasıboyutu,
                Location = new Point(locationx, locationy)
            };

            this.pnl.Controls.Add(label);//kod ile oluşturduğumuz yılan kafasınız panele ekliyoruz

            return label;
        }

        private void YılanıYerleştir()//oluşturulan yılan parçasını panelde yerleştiren method
        {
            _yilaninkafasi = YılanParçasıOluştur(0, 0);
            _yilaninkafasi.Text = ":";
            _yilaninkafasi.TextAlign = ContentAlignment.MiddleCenter;
            _yilaninkafasi.ForeColor = Color.White;
            var locationX = (pnl.Width / 2) - (_yilaninkafasi.Width / 2);
            var locationY = (pnl.Height / 2) - (_yilaninkafasi.Height / 2);
            _yilaninkafasi.Location = new Point(locationX, locationY);

        }

        private void YemOluştur()//yem oluşturma methodumuz
        {
            Label label = new Label()//yemimiz label türünde olacak aşağıda özelliklerini belirtiyoruz
            {
                Name = "yem",
                BackColor = Color.Yellow,
                Width = _yemboyutu,
                Height = _yemboyutu,

            };
            _yem = label;
            this.pnl.Controls.Add(label);//bu kod ile oluşturduğumuz yeri panel formdaki panel içine ekliyoruz
        }

        private void YeminYeriniDeğiştir()//yemin yerini rastgele değiştirdiğimiz methodumuz
        {
            var locationY = 0;
            var locationX = 0;
            bool durum;
            do
            {
                durum = false;
                locationX = _random.Next(0, pnl.Width - _yemboyutu); //yemimizin koordinatlarını rasgele ayarlıyoruz
                locationY = _random.Next(0, pnl.Height - _yemboyutu);

                var rect1 = new Rectangle(new Point(locationX, locationY), _yem.Size);//dikdörtgen oluşturuyoruz 

                foreach (Control control in pnl.Controls)//paneli kontrol ettiriyoruz
                {
                    if (control is Label && control.Name.Contains("yilanParça"))//eğer control sonucu bir label bulunursa ve bu labelın adında yılanParça geçiyorusa if içine gir
                    {
                        var rect2 = new Rectangle(control.Location, control.Size);//bu labelın koordinatlarında bir dikdörtgen oluştur

                        if (rect1.IntersectsWith(rect2))//eğer yemimiz ve yılan parçası kesişirse durum true olacak
                        {
                            durum = true;
                            break;
                        }
                    }

                }
            } while (durum);

            _yem.Location = new Point(locationX, locationY);


        }

        private enum HareketYönü
        {
            yukarı,   //hareket yönlerimizi belirtiyoruz ve yukarda HareketYönü türünde bir nesne oluşturuyoruz
            aşağı,
            sağa,
            sola
        }



        private void Form1_KeyDown(object sender, KeyEventArgs e)//kullanıcı form açıkken bir tuşa basrsa eğer bu method çalışacak
        {
            var keycode = e.KeyCode;//klavyeden basılan tuşu burada bir değişkene atıyoruz
            if (_yön == HareketYönü.sola && keycode == Keys.Right || _yön == HareketYönü.sağa && keycode == Keys.Left || _yön == HareketYönü.aşağı && keycode == Keys.Up|| _yön == HareketYönü.yukarı && keycode == Keys.Down)
            {
                return;//eğer yılan bir yöne giderse kullanıcı tam tersi yöne basarsa bu komjut algılanmaycak çünkü yılan kendi üzerinden geriye dönemez
            }

            switch (keycode)//hangi yöne basarsa switch o yeri bulacak ve _yön değişkenini
            {
                case Keys.Up:
                    _yön = HareketYönü.yukarı;
                    break;
                case Keys.Down:
                    _yön = HareketYönü.aşağı;
                    break;
                case Keys.Right:
                    _yön = HareketYönü.sağa;
                    break;
                case Keys.Left:
                    _yön = HareketYönü.sola;
                    break;


                default:
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)//timer çalışırken her bir tikte çalışır
        {
            YılanınKafasınıTakipEt();
            var locationX = _yilaninkafasi.Location.X;
            var locationY = _yilaninkafasi.Location.Y;
            switch (_yön)
            {
                case HareketYönü.yukarı:
                    _yilaninkafasi.Location = new Point(locationX, locationY - (_yilaninkafasi.Width + _yilanparçasıarasıboşluk));
                    break;
                case HareketYönü.aşağı:
                    _yilaninkafasi.Location = new Point(locationX, locationY + (_yilaninkafasi.Width + _yilanparçasıarasıboşluk));
                    break;
                case HareketYönü.sağa:
                    _yilaninkafasi.Location = new Point(locationX + (_yilaninkafasi.Width + _yilanparçasıarasıboşluk), locationY);
                    break;
                case HareketYönü.sola:
                    _yilaninkafasi.Location = new Point(locationX - (_yilaninkafasi.Width + _yilanparçasıarasıboşluk), locationY);
                    break;
                default:
                    break;
            }

            Yilanyemiyedimi();
            OyunBittiMİ();
        }

        private void OyunBittiMİ()
        {
            var rect1 = new Rectangle(_yilaninkafasi.Location, _yilaninkafasi.Size);
            bool oyunbitti = false;
            foreach (Control controls in pnl.Controls)
            {
                if (controls is Label && controls.Name!=_yilaninkafasi.Name)
                {
                    var rect2 = new Rectangle(controls.Location, controls.Size);
                    if (rect1.IntersectsWith(rect2))
                    {
                        oyunbitti = true;
                        break;
                    }
                }
            }
            if (oyunbitti)
            {
                timer1.Enabled = false;
               DialogResult cevap= MessageBox.Show("Oyun bitti,Tekrar oynamak ister misiniz?","Punınız:"+LblPuan.Text,MessageBoxButtons.YesNo);

                if (cevap==DialogResult.Yes)
                {
                    LblPuan.Text = "0";
                    YenidenBaşlat();
                }
                else
                {
                    Close();
                }
            }
        }

        private void Yilanyemiyedimi()//yılan yemi yerse çalışacak method
        {//iki dikdörtgen oluşturduk birisi yılanın kafası diğeri ise yem olcak
            var rect1 = new Rectangle(_yilaninkafasi.Location, _yilaninkafasi.Size);
            var rect2 = new Rectangle(_yem.Location, _yem.Size);

            if (rect1.IntersectsWith(rect2))//eğer bu dikdörtgenelr çarpışırsa if içine girer
            {
                LblPuan.Text = (Convert.ToInt32(LblPuan.Text) + 10).ToString();//puan artacak
                YeminYeriniDeğiştir();//yemin yerini değiştiriyoruz
                YılanParçasıOluştur(-_yemboyutu, -_yemboyutu);//ilk parçanın arkasına yeni bir parça ekliyoruz
            }
        }

        private void YılanınKafasınıTakipEt()//yılanın parçalarının birbirini takip etmesi için yazdığımız methot
        {
            if (_yılanparçasısayısı <= 1) return;//eğer yılan tek parça veya hiç yok ise bu methottan çıksın

            for (int i = _yılanparçasısayısı; i > 1; i--)//yılanparçası sayısı 1'den büyükse if içine girecek 
            {
                var sonparça = (Label)pnl.Controls[i];
                var sondanöncekiparça = (Label)pnl.Controls[i - 1];
                sonparça.Location = sondanöncekiparça.Location;    //if sondan başa doğru kontrol edecek
                                                                   //son parçanın konumunu ondan önceki parçanın konumuna eşitleyecek böylece her parça kendinden önceki parçayı takip edecek  
            }


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
