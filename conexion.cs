// AUTHOR: ALICEEILISH
// PROJETS: PROYECTO HABLADORES
// FECHA: 12/02/2022

// OPTIMIZAR CÓDIGO

using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using BarcodeLib;

namespace PH_Version_1._0
{
    public class conexion
    {
        // variables
        public string colum, colum1, colum3, colum4,colum5, columnName;
        public int  colum2, contador = 0, contador02 =0;

        // arreglos
        public static string[] stringArray = new string[10000];
        string[] stringArray1 = new string[10000];
        int[] stringArray2 = new int[10000];
        string[] stringArray3 = new string[10000];
        string[] stringArray4 = new string[10000];
        string[] stringArray5 = new string[10000];

        string query01 = @"SELECT * FROM [dbo].[@DK_ALMACEN]";
        string query = @"select ART.ItemCode, ART.ItemName, ART.FirmCode, ART.FirmName, U_DK_GARANTIA, isNull(ART.CodeBars,0), PRE.PriceList1, PRE.ListaName1, PRE.Price1, PRE.PriceList2, PRE.ListaName2, PRE.Price2, ART.Name from
(select A.ItemCode, A.ItemName, A.FirmCode, M.FirmName, A.U_DK_GARANTIA, A.CodeBars, F.name from OITM A 
join OMRC M on A.FirmCode = M.FirmCode
join ITM1 P on A.ItemCode = P.ItemCode
left join [@FAMILIA_PROD] F on f.Code = a.U_Familia
where a.SellItem = 'Y' and SUBSTRING(a.itemcode,1,1) = 'L' and A.OnHand > 0
group by A.ItemCode, A.ItemName, A.FirmCode, M.FirmName, A.U_DK_GARANTIA, A.CodeBars, F.Name) ART
join
(select P1.ItemCode1 as ItemCode, P1.PriceList1, P1.ListaName1, P1.Price1, P2.PriceList2, P2.ListaName2, P2.Price2 from 
(select A.ItemCode ItemCode1, P.PriceList PriceList1, L1.ListName ListaName1, P.Price Price1
from OITM A 
join ITM1 P on A.ItemCode = P.ItemCode
join OPLN L1 on P.PriceList = L1.ListNum
where a.SellItem = 'Y' and SUBSTRING(a.itemcode,1,1) = 'L' and P.PriceList = 7) P1
left join
(select A.ItemCode ItemCode2, P.PriceList PriceList2, L1.ListName ListaName2, P.Price Price2
from OITM A 
join ITM1 P on A.ItemCode = P.ItemCode
join OPLN L1 on P.PriceList = L1.ListNum
where a.SellItem = 'Y' and SUBSTRING(a.itemcode,1,1) = 'L' and P.PriceList = 4) P2
on P1.ItemCode1 = P2.ItemCode2) PRE
on ART.ItemCode = PRE.ItemCode"; // ----------------------------------------------------------- query requerido -----------------------------------------------------------

        // ----------------------------------------------------------------------cadena de conexión al servidor----------------------------------------------------------------------
        public SqlConnection conexion_server = new SqlConnection(@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=DB_TIENDASDAKA_PROD;Data Source=SERV-SAP\SRVSAP2012");
        public void funcion_conexion()
        {
            conexion_server.Open();
        }

        public void funcion_consulta(string n, string n1)
        {
            funcion_conexion();
            try
            {
                SqlCommand cmd = new SqlCommand(query, conexion_server);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // se obtiene las columnas de la tabla en la base de datos(SqlSever) exaple(name = getString(colum = 0 / ItemCode)
                    colum = reader.GetString(0).ToString();
                    colum1 = reader.GetString(1).ToString();
                    colum2 = reader.GetInt16(2);
                    colum3 = reader.GetDecimal(4).ToString();
                    colum4 = reader.GetString(5).ToString();
                    colum5 = reader.GetDecimal(8).ToString(); // price1
                    columnName = reader.GetString(3).ToString();

                    stringArray[contador] = colum;
                    stringArray1[contador] = colum1;
                    stringArray2[contador] = colum2;
                    stringArray3[contador] = colum3;
                    stringArray4[contador] = columnName;
                    stringArray5[contador] = colum5;

                    contador = contador + 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
            finally
            {
                conexion_server.Close();
            }

            // generarPDF
            Document doc = new Document();
            doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(@"C:\Users\d.marcano\Desktop\habladores_pruebas.pdf", FileMode.Create));
            doc.Open();
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _specialFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _special01Font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _id = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.GREEN);

            string num0 = n;
            int x = Int32.Parse(num0);

            // ia para la generación de documentos pdf
            decimal paginas, hs = obtner02.valor_habladoresS, vuelta = 0;
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

            // en este flujo controlamos las paginas y tambien la escritura inteligente
            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(@"C:\Users\d.marcano\Desktop\bar_code.png"); // getDirectory
            iTextSharp.text.Image image2 = iTextSharp.text.Image.GetInstance(@"C:\Users\d.marcano\Desktop\image\LOGO PIDA (1,17x0,9cm).png"); // Logo

            decimal reciduo = hs;
            while (vuelta > 0) {
                if (reciduo > 4 || reciduo == 4)
                {
                    // Primera posición
                    doc.NewPage();
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray[x]), 260.000f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray1[x]), 141.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray3[x]), 200.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray4[x]), 141.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$" + stringArray5[x]), 70f, 470f, 0); // price1 
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(200.000f, 455.276f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(250.000f, 525.276f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);

                    // Segunda posición
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray[100]), 700.732f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray1[100]), 630.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray3[100]), 650.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray4[100]), 630.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$" + stringArray5[100]), 510f, 470f, 0); // price1
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(650.732f, 455.276f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(710.732f, 525.276f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);

                    // Tercera posición
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray[200]), 260.000f, 100.000f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray1[200]), 141.732f, 100.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray3[200]), 200.000f, 100.000f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray4[200]), 141.732f, 100.000f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$" + stringArray5[100]), 70f, 100f, 0); // price1
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(200.000f, 100.000f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(250.000f, 100.000f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);

                    // Cuarta posición
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray[200]), 700.732f, 100.000f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray1[200]), 630.732f, 100.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray3[200]), 650.000f, 100.000f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray4[200]), 630.732f, 100.000f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$" + stringArray5[100]), 510f, 100.000f, 0); // price1
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
                    // Primera posición
                    doc.NewPage();
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray[x]), 260.000f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray1[x]), 141.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray3[x]), 200.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray4[x]), 141.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$" + stringArray5[x]), 70f, 470f, 0); // price1 
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(200.000f, 455.276f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(250.000f, 525.276f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);

                    // Segunda posición
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray[100]), 700.732f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray1[100]), 630.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray3[100]), 650.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray4[100]), 630.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$" + stringArray5[100]), 510f, 470f, 0); // price1
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(650.732f, 455.276f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(710.732f, 525.276f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);

                    // Tercera posición
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray[200]), 260.000f, 100.000f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray1[200]), 141.732f, 100.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray3[200]), 200.000f, 100.000f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray4[200]), 141.732f, 100.000f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$" + stringArray5[100]), 70f, 100f, 0); // price1
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(200.000f, 100.000f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(250.000f, 100.000f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);
                    reciduo = reciduo - 3;

                } else if (reciduo == 2)
                {
                    // Primera posición
                    doc.NewPage();
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray[x]), 260.000f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray1[x]), 141.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray3[x]), 200.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray4[x]), 141.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$" + stringArray5[x]), 70f, 470f, 0); // price1 
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(200.000f, 455.276f); // posicion de la imagen (x,y);

                    image2.ScaleAbsoluteWidth(68.0315f); // ancho de la imagen;
                    image2.ScaleAbsoluteHeight(56.6929f); // alto de la imagen;
                    image2.SetAbsolutePosition(250.000f, 525.276f); // posicion de la imagen (x,y);
                    doc.Add(image1);
                    doc.Add(image2);

                    // Segunda posición
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray[100]), 700.732f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray1[100]), 630.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray3[100]), 650.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray4[100]), 630.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$" + stringArray5[100]), 510f, 470f, 0); // price1
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(650.732f, 455.276f); // posicion de la imagen (x,y);
                    reciduo = reciduo - 2;
                } else if(reciduo > 0 && reciduo <= 1)
                {
                    // Primera posición
                    doc.NewPage();
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray[x]), 260.000f, 490.276f, 0); // ItemCode
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray1[x]), 141.732f, 549.000f, 0); // ItemName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray3[x]), 200.000f, 425.197f, 0); // D_K.Garantía
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray4[x]), 141.732f, 566.929f, 0); // FirmName
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$" + stringArray5[x]), 70f, 470f, 0); // price1 
                    image1.ScaleAbsoluteWidth(130); // ancho de la imagen;
                    image1.ScaleAbsoluteHeight(30); // alto de la imagen;
                    image1.SetAbsolutePosition(200.000f, 455.276f); // posicion de la imagen (x,y);
                    reciduo = reciduo - 1;
                } else if (reciduo < 0)
                {
                    break;
                }
                vuelta--;
            }
            
            string variable_x = stringArray5[x];

            // solucion al código de barras
            obtener.valor_obtenido = variable_x;
            doc.Close();
        }

        public string funcion_busqueda() {
            return query01;
        }

        public string funcion_variable_n(string n, string n1)
        {
            string variable_n = n;
            string variable_n1 = n1;
            funcion_consulta(variable_n,variable_n1);
            return n;
        }

        public static void funcion_variable_x(string x)
        {
            string variable_x = x;
        }

        public string funcion_ui() {
           return query;
        }

    }
}

