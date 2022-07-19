// AUTHOR: ALICEEILISH
// PROJETS: PROYECTO HABLADORES
// FECHA: 12/02/2022

// OPTIMIZAR CÓDIGO

using System.Data.SqlClient;

namespace PH_Version_1._0
{
    public class conexion
    {
        public static string query = @"select ART.ItemCode, ART.ItemName, ART.FirmCode, ART.FirmName, U_DK_GARANTIA, isNull(ART.CodeBars,0), PRE.PriceList1, PRE.ListaName1, PRE.Price1, PRE.PriceList2, PRE.ListaName2, PRE.Price2, ART.Name from
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
    }
}

