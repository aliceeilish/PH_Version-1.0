// AUTHOR: ALICEEILISH
// PROJETS: PROYECTO HABLADORES
// FECHA: 12/02/2022

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
        public int  colum2, contador = 0;

        // arreglos
        public static string[] stringArray = new string[10000];
        string[] stringArray1 = new string[10000];
        int[] stringArray2 = new int[10000];
        string[] stringArray3 = new string[10000];
        string[] stringArray4 = new string[10000];
        string[] stringArray5 = new string[10000];

        public static string[] stringArray6 = new string[10000];
        string[] stringArray7 = new string[10000];
        int[] stringArray8 = new int[10000];
        string[] stringArray9 = new string[10000];
        string[] stringArray10 = new string[10000];
        string[] stringArray11 = new string[10000];

        public static string[] stringArray12 = new string[10000];
        string[] stringArray13 = new string[10000];
        int[] stringArray14 = new int[10000];
        string[] stringArray15 = new string[10000];
        string[] stringArray16 = new string[10000];
        string[] stringArray17 = new string[10000];

        public static string[] stringArray18 = new string[10000];
        string[] stringArray19 = new string[10000];
        int[] stringArray20 = new int[10000];
        string[] stringArray21 = new string[10000];
        string[] stringArray22 = new string[10000];
        string[] stringArray23 = new string[10000];

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

                    stringArray6[contador] = colum;
                    stringArray7[contador] = colum1;
                    stringArray8[contador] = colum2;
                    stringArray9[contador] = colum3;
                    stringArray10[contador] = columnName;
                    stringArray11[contador] = colum5;

                    stringArray12[contador] = colum;
                    stringArray13[contador] = colum1;
                    stringArray14[contador] = colum2;
                    stringArray15[contador] = colum3;
                    stringArray16[contador] = columnName;
                    stringArray17[contador] = colum5;

                    stringArray18[contador] = colum;
                    stringArray19[contador] = colum1;
                    stringArray20[contador] = colum2;
                    stringArray21[contador] = colum3;
                    stringArray22[contador] = columnName;
                    stringArray23[contador] = colum5;
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
            doc.SetPageSize(new iTextSharp.text.Rectangle(612f, 792f)); // 15x9,5(425.197f, 269.291f) en puntos Tipo graficos | 11x6,5(311.811f,184.252f) en puntos Tipo graficos
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(@"C:\Users\d.marcano\Desktop\PH_Version_1.0.pdf", FileMode.Create));
            doc.Open();
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _specialFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _special01Font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _id = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.GREEN);
            //Paragraph title = new Paragraph();
            //title.Font = FontFactory.GetFont(FontFactory.TIMES, 20f, BaseColor.BLACK);
            //title.Add();
            //doc.Add(title);

            string num0 = n;
            int x = Int32.Parse(num0);

            string num1 = n1;
            //int x1 = Int32.Parse(num1);
            
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray[x],_id), 283.465f, 600f, 0); // ItemCode
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray1[x], _standardFont), 283.465f, 400f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray2[x]), 283.465f, 500f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray3[x], _standardFont), 283.465f, 300f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray4[x]), 283.465f, 500f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$"+stringArray5[x], _special01Font), 130, 200f, 0); // price1 
            
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray6[100], _id), 200.465f, 293.465f, 0); //
             ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray7[100]), 200.465f, 260.465f, 0); //
             ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray8[100]), 200.465f, 250.400f, 0); //
             ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray9[100]), 200.465f, 240.350f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray10[100]), 200.465f, 230.340f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$"+stringArray11[100], _special01Font), 130, 200f, 0); // barcode

            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray12[200], _id), 200.465f, 293.465f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray13[200]), 200.465f, 260.465f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray14[200]), 200.465f, 250.400f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray15[200]), 200.465f, 240.350f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray16[200]), 200.465f, 230.340f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$"+stringArray17[200], _special01Font), 130, 200f, 0); //barcode

            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray13[100], _id), 200.465f, 293.465f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray14[100]), 200.465f, 260.465f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray15[100]), 200.465f, 250.400f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray16[100]), 200.465f, 240.350f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase(stringArray17[100]), 200.465f, 230.340f, 0); //
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_CENTER, new Phrase("$"+stringArray18[100], _special01Font), 200f, 240f, 0); //barcode
            string variable_x = stringArray5[x];
            //int code_bars = variable_x;
            //string variable_xyz = variable_x.ToString();

            // solucion al código de barras
            obtener.valor_obtenido = variable_x;
            //funcion_variable_x(variable_x);

            // atributos del PDF
            //
            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(@"C:\Users\d.marcano\Desktop\bar_code.png"); // getDirectory

            //image1.ScalePercent(50f);
            image1.ScaleAbsoluteWidth(175); // ancho de la imagen;
            image1.ScaleAbsoluteHeight(30); // alto de la imagen;
            image1.SetAbsolutePosition(100, 100); // posicion de la imagen (x,y);

            doc.Add(image1);
            doc.Close();
        }

        public void  funcion_busquedaDinamica()
        {
            funcion_conexion();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select A.ItemCode, A.ItemName
from OITM A 
WHERE A.ItemCode LIKE '%LB-00000001' ", conexion_server);
                SqlDataReader reader = cmd.ExecuteReader();
               // MessageBox.Show(reader.GetString(0).ToString());
                // MessageBox.Show(reader.GetString(1).ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show("fail searh" + ex.Message);
            }
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

