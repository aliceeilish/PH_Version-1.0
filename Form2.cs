// AUTHOR: ALICEEILISH
// PROJETS: PROYECTO HABLADORES
// FECHA: 12/02/2022

using System.Data;
using System.Data.SqlClient;
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace PH_Version_1._0
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SqlConnection cadena_conexion = new SqlConnection(@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=DB_TIENDASDAKA_PROD;Data Source=SERV-SAP\SRVSAP2012");
       
        private void Form2_Load(object sender, EventArgs e)
        {
            // --------------------------------------------------------------------------------------------------------------------------------
            comboBox1.Items.Add("Filtrar por nombre");
            comboBox1.Items.Add("Filtrar por código");
            // -------------------------------------------------------------------------------------------------------------------------------

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(conexion.query, cadena_conexion);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                this.dataGridView1.Columns["FirmCode"].Visible = false;
                this.dataGridView1.Columns["FirmName"].Visible = false;
                this.dataGridView1.Columns["U_DK_GARANTIA"].Visible = false;
                this.dataGridView1.Columns["Column1"].Visible = false;
                this.dataGridView1.Columns["PriceList1"].Visible = false;
                this.dataGridView1.Columns["PriceList2"].Visible = false;
                this.dataGridView1.Columns["U_DK_GARANTIA"].Visible = false;
                this.dataGridView1.Columns["Price1"].Visible = false;
                this.dataGridView1.Columns["Price2"].Visible = false;
                this.dataGridView1.Columns["Name"].Visible = false;
                this.dataGridView1.Columns["ListaName1"].Visible = false;
                this.dataGridView1.Columns["ListaName2"].Visible = false;
                this.dataGridView1.Columns["ItemName"].Width = 700;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //-------VARIABLES Y ARREGLOS:
            string[] columna00 = new string[10000], columna01 = new string[10000]; // columnas del datagridview
            string[] lista_dinamica = new string[10000];
            //string [] columna02 = new string[10000];
            //string[] columna03 = new string[10000];
            //string[] columna04 = new string[10000];
            //string[] columna05 = new string[10000];
            //string[] columna06 = new string[10000];

            string n = "0", bandera="";
            decimal hs = 0;

            //-------SELECCIÓN DE HABLADORES:
            Int32 selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                for (int fila = 0; fila < dataGridView1.Rows.Count - 1; fila++)
                {
                    columna00[fila] = dataGridView1.Rows[fila].Cells[0].Value.ToString();
                    columna01[fila] = dataGridView1.Rows[fila].Cells[1].Value.ToString();
                    //columna02[fila] = dataGridView1.Rows[fila].Cells[2].Value.ToString(); //el error se debe al tipo de dato
                    //columna03[fila] = dataGridView1.Rows[fila].Cells[3].Value.ToString(); //obtener la información sin romper el programa
                    //columna04[fila] = dataGridView1.Rows[fila].Cells[4].Value.ToString();
                    //columna06[fila] = dataGridView1.Rows[fila].Cells[5].Value.ToString();
                    //columna05[fila] = dataGridView1.Rows[fila].Cells[8].Value.ToString();

                    for (int i = 0; i < selectedRowCount ; i++)
                    {
                        lista_dinamica[i] = dataGridView1.SelectedRows[i].Index.ToString(); //valor encargado de la mecanica dinamica / lista sde valores
                    }
                    hs = selectedRowCount;
                }
            }

           
            MessageBox.Show(""+hs);
            //-------CARPETA PARA EL ARCHIVO HABLADORES:
            string path01 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TI.DAKA-HABALDORES\";
            string dt = DateTime.Now.ToString("dd -MM-yyyy H-mm-ss"); // obtener fecha actual

            string folderPath = path01;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //-------ARMADO DE FICHERO PDF:
            int x = Int32.Parse(n);

            // CÓDIGO DE BARRAS
            BarcodeLib.Barcode codeBars = new BarcodeLib.Barcode();
            codeBars.IncludeLabel = true;
            panel1.BackgroundImage = codeBars.Encode(BarcodeLib.TYPE.CODE128, "indifine0002", Color.Black, Color.White, 300, 100);

            System.Drawing.Image ImageCodeBars = (System.Drawing.Image)panel1.BackgroundImage.Clone();
            SaveFileDialog SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.AddExtension = true;
            SaveFileDialog.Filter = "Image PNG (*.png) | *.png";
            ImageCodeBars.Save(SaveFileDialog.InitialDirectory = path01 + @"bar_code.png"); // DONDE GUARDAR LA IMAGEN CÓDIGO

            // PDF
            Document doc = new Document();
            doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path01 + dt + @" TI.DAKA-HABLADOR_G.pdf", FileMode.Create)); // DIRECIÓN DEL FICHERO PDF
            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(path01 + @"bar_code.png"); // DIRECCIÓN IMAGEN CÓDIGO
            iTextSharp.text.Image image2 = iTextSharp.text.Image.GetInstance(@"C:\Users\d.marcano\Desktop\image\logo\LOGO PIDA (1,17x0,9cm).png"); // DIRECCIÓN IMAGEN LOGO, ACCEDER SIEMPRE A LA CARPETA IMAGENES
            doc.Open(); 

            // IA PARA LA DIVISIÓN DE PAGINAS SEGÚN LOS HABLADORES SOLICITADOS
            decimal paginas, vuelta = 0;
            if (hs <= 4)
            {
                vuelta = 1;
            }
            else
            {
                paginas = hs / 4;

                while (paginas > 0)
                {
                    if (paginas > 0.20M)
                    {
                        if (paginas >= 1)
                        {
                            vuelta++;
                            paginas = paginas - 1;
                        }

                        if (paginas > 0.20M && paginas < 1)
                        {
                            vuelta++;
                            break;
                        }
                    }
                }
            }

            // lograr cambiar el valor dinamicamente de x desde una lista

            //int i = 0;
            //foreach (var valor in lista_dinamica)
            //{
                //if (bandera == "")
                //{
                    //x = valor[i];
                    //bandera = "1";
                //}

                //if (bandera == "1")
                //{
                    //x = valor[i];
                    //bandera = "2";
                //}
                //i++;
            //}

            // ESCRITURA INTELIGENTE SEGÚN EL NÚMERO DE PAGINAS Y HABLADORES
            decimal reciduo = hs;
            while (vuelta > 0)
            {
                if (reciduo > 4 || reciduo == 4)
                {
                    // 1
                    doc.NewPage();
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna00[x]), 260.000f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna01[x]), 141.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 200.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 141.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$"), 70f, 470f, 0); // price1 
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(200.000f, 455.276f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(250.000f, 525.276f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);

                    // 2
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna00[x]), 700.732f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna01[x]), 630.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 650.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 630.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$"), 510f, 470f, 0); // price1
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(650.732f, 455.276f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(710.732f, 525.276f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);
                
                    // 3
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna00[x]), 260.000f, 100.000f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna01[x]), 141.732f, 100.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 200.000f, 100.000f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 141.732f, 100.000f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 70f, 100f, 0); // price1
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(200.000f, 100.000f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(250.000f, 100.000f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);

                    // 4
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna00[x]), 700.732f, 100.000f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna01[x]), 630.732f, 100.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 650.000f, 100.000f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 630.732f, 100.000f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 510f, 100.000f, 0); // price1
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(650.732f, 100.000f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(710.732f, 100.000f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);
                    reciduo = reciduo - 4;
                }
                else if (reciduo == 3)
                {
                    // 1
                    doc.NewPage();
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna00[x]), 260.000f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna01[x]), 141.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 200.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 141.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$"), 70f, 470f, 0); // price1 
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(200.000f, 455.276f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(250.000f, 525.276f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);

                    // 2
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna00[x]), 700.732f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna01[x]), 630.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 650.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 630.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 510f, 470f, 0); // price1
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(650.732f, 455.276f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(710.732f, 525.276f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);

                    // 3
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna00[x]), 260.000f, 100.000f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna01[x]), 141.732f, 100.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 200.000f, 100.000f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 141.732f, 100.000f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$"), 70f, 100f, 0); // price1
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(200.000f, 100.000f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(250.000f, 100.000f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);
                    reciduo = reciduo - 3;
                }
                else if (reciduo == 2)
                {
                    // 1
                    doc.NewPage();
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna00[x]), 260.000f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna01[x]), 141.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 200.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 141.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$"), 70f, 470f, 0); // price1 
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(200.000f, 455.276f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(250.000f, 525.276f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);

                    // 2
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna00[x]), 700.732f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna01[x]), 630.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 650.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 630.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$"), 510f, 470f, 0); // price1
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(650.732f, 455.276f); // posicion de la imagen (x,y);
                    reciduo = reciduo - 2;

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(710.732f, 525.276f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);
                }
                else if (reciduo > 0 && reciduo <= 1)
                {
                    // 1
                    doc.NewPage();
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna00[x]), 260.000f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(columna01[x]), 141.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 200.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(""), 141.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$"), 70f, 470f, 0); // price1 
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(200.000f, 455.276f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(250.000f, 525.276f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);
                    reciduo = reciduo - 1;
                }
                else if (reciduo < 0)
                {
                    break;
                }
                vuelta--;
            }
            doc.Close();
            MessageBox.Show("Terminado");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cadena_conexion.Open();
                SqlCommand cmd = cadena_conexion.CreateCommand();
                cmd.CommandText = "select A.ItemCode, A.ItemName from OITM A WHERE ("+obtener.valor_busqueda+") LIKE ('"+textBox1.Text+"%')";
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch
            {
                textBox1.Text = "";
            }
            finally
            {
                cadena_conexion.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                obtener.valor_busqueda = "A.ItemName";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                obtener.valor_busqueda = "A.ItemCode";
            }
        }
    }

    class obtener
    {
        public static string valor_obtenido = "NULL";
        public static string valor_busqueda = "A.ItemCode";
    }
}
