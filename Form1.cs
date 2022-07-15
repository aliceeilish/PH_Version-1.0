// AUTHOR: ALICEEILISH
// PROJETS: PROYECTO HABLADORES
// FECHA: 12/02/2022

namespace PH_Version_1._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Lista Estandar");
            comboBox2.Items.Add("Promo Daka");
        }
        private void button1_Click(object sender, EventArgs e)
        {
                // ---------------------------------------------------------------------------------------------------------------------------
                conexion n = new conexion();
                Form2 form2 = new Form2();
                form2.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }
    }

    class obtner02
    {
        public static int valor_habladoresS = 0;
    }
}