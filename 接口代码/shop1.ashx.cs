using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using XTBase;

namespace MESProject.captcha
{
    /// <summary>
    /// shop1 的摘要说明
    /// </summary>
    public class shop1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.QueryString["Method"].ToString();
            if (!string.IsNullOrWhiteSpace(method))
            {
                MethodInfo methodInfo = this.GetType().GetMethod(method);
                object[] para = { context };
                string strRtn = (methodInfo.Invoke(this, para)).ToString();
                context.Response.ContentType = "text/plain";
                context.Response.Write(strRtn);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public string version(HttpContext context)
        {
            string userid = context.Request["str1"] == null ? "" : context.Request["str1"].ToString();//获取参数
            string strResult = "";
            string sql = @"SELECT VALUE
                    FROM SYS_PARA A
                    WHERE A.CODE = 'VER'";
            string strVer = (DbHelperOra.GetSingle(sql) ?? "").ToString();
            strResult = new JObject(
                             new JProperty("version", strVer),
                             new JProperty("url", ""),
                             new JProperty("apkurl", "h"),
                             new JProperty("note", "更新提示！")
                         ).ToString();
            return strResult;
        }
        public string addres(HttpContext context)
        {
            string userid = context.Request["str1"] == null ? "" : context.Request["str1"].ToString();//获取参数
            string strResult = "";
            string sql = string.Format(@"SELECT *
                            FROM DOC_ADDRESS WHERE FUSERID = '{0}'", userid);
            DataTable dt = DbHelperOra.QueryForTable(sql);
            if (dt.Rows.Count == 0)
            {
                strResult = new JObject(
                            new JProperty("res", "0"),
                            new JProperty("msg", "用户为空！")
                        ).ToString();
            }
            else
            {
                //校验账户
                string strJson = JsonConvert.SerializeObject(dt);
                strResult = new JObject(
                            new JProperty("res", "1"),
                            new JProperty("msg", strJson)
                        ).ToString();
            }
            return strResult;
        }
        public string addresDel(HttpContext context)
        {
            string seqno = context.Request["seqno"] == null ? "" : context.Request["seqno"].ToString();//获取参数
            string strResult = "";
            string sql = string.Format(@"DELETE FROM DOC_ADDRESS WHERE FSEQNO = '{0}'", seqno);
            if (DbHelperOra.ExecuteSql(sql) < 1)
            {
                strResult = new JObject(
                            new JProperty("res", "0"),
                            new JProperty("msg", "无数据")
                        ).ToString();
            }
            else
            {
                strResult = new JObject(
                            new JProperty("res", "1"),
                            new JProperty("msg", "")
                        ).ToString();
            }
            return strResult;
        }
        public string addresadd(HttpContext context)
        {
            string userid = context.Request["str1"] == null ? "" : context.Request["str1"].ToString();//获取参数
            string name = context.Request["name"] == null ? "" : context.Request["name"].ToString();//获取参数;
            string ads = context.Request["ads"] == null ? "" : context.Request["ads"].ToString();//获取参数
            string tel = context.Request["tel"] == null ? "" : context.Request["tel"].ToString();//获取参数
            string area = context.Request["area"] == null ? "" : context.Request["area"].ToString();//获取参数
            string seqno = context.Request["seqno"] == null ? "" : context.Request["seqno"].ToString();//获取参数
            string strResult = "";

            if (!string.IsNullOrEmpty(seqno))
            {
                string sqlDel = string.Format(@"DELETE FROM DOC_ADDRESS WHERE FSEQNO = '{0}'", seqno);
                DbHelperOra.ExecuteSql(sqlDel);
            }
            else
            {
                seqno = (DbHelperOra.GetSingle("select seq_address.nextval from dual")).ToString();
            }
            string sql = "";

            if (DbHelperOra.ExecuteSql(sql) > 0)
            {
                strResult = new JObject(
                            new JProperty("res", "1"),
                            new JProperty("msg", "")
                        ).ToString();
            }
            else
            {
                strResult = new JObject(
                            new JProperty("res", "0"),
                            new JProperty("msg", "")
                        ).ToString();
            }
            return strResult;
        }
        public string addresIdx(HttpContext context)
        {
            string seqno = context.Request["str1"] == null ? "" : context.Request["str1"].ToString();//获取参数
            string strResult = "";

            string sql = string.Format(@"SELECT * FROM DOC_ADDRESS WHERE FSEQNO = '{0}'", seqno);
            DataTable dt = DbHelperOra.QueryForTable(sql);
            if (dt.Rows.Count > 0)
            {
                strResult = new JObject(
                            new JProperty("res", "1"),
                            new JProperty("msg", JsonConvert.SerializeObject(dt))
                        ).ToString();
            }
            else
            {
                strResult = new JObject(
                            new JProperty("res", "0"),
                            new JProperty("msg", "")
                        ).ToString();
            }
            return strResult;
        }
        public string goodslist(HttpContext context)
        {
            string page = context.Request["page"] == null ? "" : context.Request["page"].ToString();//获取参数
            string strUrl = System.Configuration.ConfigurationManager.AppSettings["URL_PIC"];
            string sql = string.Format(@"SELECT A.*, '{0}'||A.FGDSEQ||'/'|| B.FNAME FURL
                          FROM DOC_GOODS A
                          LEFT JOIN(SELECT *
                                       FROM DOC_GOODSPIC
                                      WHERE(FGDSEQ, FROWNO) IN
                                            (SELECT FGDSEQ, MIN(FROWNO) FROWNO
                                               FROM DOC_GOODSPIC WHERE FTYPE = 'T1'
                                              GROUP BY FGDSEQ) AND FTYPE = 'T1') B ON A.FGDSEQ = B.FGDSEQ
                         WHERE A.FFLAG = 'Y' ORDER BY A.FGDSEQ", strUrl);

            int total = 0;
            string strResult = "";
            DataTable dt = PubFunc.DbGetPage(Convert.ToInt16(page) - 1, 8, sql, ref total);
            if (dt.Rows.Count > 0)
            {
                strResult = new JObject(
                            new JProperty("res", "1"),
                            new JProperty("page", total),
                            new JProperty("msg", JsonConvert.SerializeObject(dt))
                        ).ToString();
            }
            else
            {
                strResult = new JObject(
                            new JProperty("res", "0"),
                            new JProperty("page", 0),
                            new JProperty("msg", "")
                        ).ToString();
            }
            return strResult;
        }
        public string goodsDetail(HttpContext context)
        {
            string fgdseq = context.Request["fgdseq"] == null ? "" : context.Request["fgdseq"].ToString();//获取参数
            string strUrl = System.Configuration.ConfigurationManager.AppSettings["URL_PIC"];
            string strResult = "";
            string sql = @"SELECT A.*,B.FTYPE, '{1}'||A.FGDSEQ||'/'||B.FNAME FURL
                            FROM DOC_GOODS A,DOC_GOODSPIC B
                            WHERE A.FGDSEQ = B.FGDSEQ AND A.FGDSEQ = '{0}' AND B.FTYPE IN('T2','T3','T4')";
            DataTable dt = DbHelperOra.QueryForTable(string.Format(sql, fgdseq, strUrl));
            if (dt.Rows.Count > 0)
            {
                strResult = new JObject(
                            new JProperty("res", "1"),
                            new JProperty("swiperItems", JsonConvert.SerializeObject(dt.Select("FTYPE = 'T2'"))),
                            new JProperty("goodsbuy", JsonConvert.SerializeObject(dt.Select("FTYPE = 'T4'"))),
                            new JProperty("goodspic", JsonConvert.SerializeObject(dt.Select("FTYPE = 'T3'")))
                        ).ToString();
            }
            else
            {
                strResult = new JObject(
                            new JProperty("res", "0")
                        ).ToString();
            }
            return strResult;
        }
        public string goodsCheckShop(HttpContext context)
        {
            string fuser = context.Request["fuser"] == null ? "" : context.Request["fuser"].ToString();//获取参数
            string strResult = "";
            string sql = @"SELECT 1
                        FROM DAT_SHOP A
                        WHERE A.FSL > 100000 AND A.FUSERID = '{0}'
                        UNION ALL
                        SELECT 1
                        FROM DAT_SHOP A
                        WHERE ROWNUM > 100 AND A.FUSERID = '{0}'";

            if (!DbHelperOra.Exists(string.Format(sql, fuser)))
            {
                strResult = new JObject(
                            new JProperty("res", "1"),
                            new JProperty("msg", "")
                        ).ToString();
            }
            else
            {
                strResult = new JObject(
                           new JProperty("res", "0"),
                           new JProperty("msg", "系统错误")
                       ).ToString();
            }
            return strResult;
        }
        public string goodsShoplist(HttpContext context)
        {
            string fuser = context.Request["fuser"] == null ? "" : context.Request["fuser"].ToString();//获取参数
            string strUrl = System.Configuration.ConfigurationManager.AppSettings["URL_PIC"];

            string strResult = "";
            string sql = @"SELECT B.FGDSEQ,B.FGDNAME,B.FZKJJ,A.FSL,'{1}'||C.FGDSEQ||'/'||C.FNAME FURL
                            FROM DAT_SHOP A,DOC_GOODS B,DOC_GOODSPIC C
                            WHERE A.FGDSEQ = B.FGDSEQ
                            AND B.FGDSEQ = C.FGDSEQ AND C.FTYPE = 'T4'
                            AND A.FUSERID = '{0}'";

            DataTable dt = DbHelperOra.QueryForTable(string.Format(sql, fuser, strUrl));
            if (dt.Rows.Count > 0)
            {
                strResult = new JObject(
                            new JProperty("res", "1"),
                            new JProperty("goodsShoplist", JsonConvert.SerializeObject(dt)),
                            new JProperty("msg", "")
                        ).ToString();
            }
            else
            {
                strResult = new JObject(
                           new JProperty("res", "0"),
                           new JProperty("goodsShoplist", ""),
                           new JProperty("msg", "系统错误")
                       ).ToString();
            }
            return strResult;
        }
        public string goodsShopChange(HttpContext context)
        {
            string fgdseq = context.Request["fgdseq"] == null ? "" : context.Request["fgdseq"].ToString();//获取参数
            string fuser = context.Request["fuser"] == null ? "" : context.Request["fuser"].ToString();//获取参数
            string fsl = context.Request["fsl"] == null ? "" : context.Request["fsl"].ToString();//获取参数

            string strResult = "";
            string sql = @"UPDATE DAT_SHOP A SET A.FSL = {2}
                            WHERE A.FGDSEQ ='{0}' AND A.FUSERID = '{1}'";

            if (DbHelperOra.ExecuteSql(string.Format(sql, fgdseq, fuser, fsl)) > 0)
            {
                strResult = new JObject(
                            new JProperty("res", "1"),
                            new JProperty("msg", "")
                        ).ToString();
            }
            else
            {
                strResult = new JObject(
                           new JProperty("res", "0"),
                           new JProperty("msg", "系统错误")
                       ).ToString();
            }
            return strResult;
        }
        public string goodsShopDel(HttpContext context)
        {
            string fgdseq = context.Request["fgdseq"] == null ? "" : context.Request["fgdseq"].ToString();//获取参数
            string fuser = context.Request["fuser"] == null ? "" : context.Request["fuser"].ToString();//获取参数

            string strResult = "";
            string sql = @"DELETE FROM DAT_SHOP A WHERE A.FGDSEQ ='{0}' AND A.FUSERID = '{1}'";

            if (DbHelperOra.ExecuteSql(string.Format(sql, fgdseq, fuser)) > 0)
            {
                strResult = new JObject(
                            new JProperty("res", "1"),
                            new JProperty("msg", "")
                        ).ToString();
            }
            else
            {
                strResult = new JObject(
                           new JProperty("res", "0"),
                           new JProperty("msg", "系统错误")
                       ).ToString();
            }
            return strResult;
        }
        public string shopaddress(HttpContext context)
        {
            string fuserid = context.Request["fuserid"] == null ? "" : context.Request["fuserid"].ToString();//获取参数
            string faddress = context.Request["faddress"] == null ? "" : context.Request["faddress"].ToString();//获取参数

            string strResult = "";
            DataTable dt;
            if (string.IsNullOrEmpty(faddress))
            {
                string sql = @"SELECT A.FUSERNAME,A.FTEL,A.FADDRESS,A.FSEQNO
                FROM DOC_ADDRESS A
                WHERE A.FUSERID = '{0}' AND rownum=1";
                dt = DbHelperOra.QueryForTable(string.Format(sql, fuserid));
            }
            else
            {
                string sql = @"SELECT A.FUSERNAME,A.FTEL,A.FADDRESS,A.FSEQNO
                FROM DOC_ADDRESS A
                WHERE A.FUSERID = '{0}' AND FSEQNO = {1}";
                dt = DbHelperOra.QueryForTable(string.Format(sql, fuserid, faddress));
            }

            if (dt.Rows.Count > 0)
            {
                strResult = new JObject(
                            new JProperty("res", "1"),
                            new JProperty("msg", JsonConvert.SerializeObject(dt))
                        ).ToString();
            }
            else
            {
                strResult = new JObject(
                           new JProperty("res", "0"),
                           new JProperty("msg", "系统错误")
                       ).ToString();
            }
            return strResult;
        }
        public string goodsorder(HttpContext context)
        {
            string fuserid = context.Request["fuserid"] == null ? "" : context.Request["fuserid"].ToString();//获取参数
            string faddress = context.Request["faddress"] == null ? "" : context.Request["faddress"].ToString();//获取参数
            string fgdseq = context.Request["fgdseq"] == null ? "" : context.Request["fgdseq"].ToString();//获取参数

            OracleParameter[] parameters = {
                                               new OracleParameter("V_USER", OracleDbType.Varchar2),
                                               new OracleParameter("V_ADDRESS", OracleDbType.Varchar2),
                                               new OracleParameter("V_GDSEQ",OracleDbType.Varchar2)};
            parameters[0].Value = fuserid;
            parameters[1].Value = faddress;
            parameters[2].Value = fgdseq;
            DbHelperOra.RunProcedure("P_GOODSORDER", parameters);

            string strResult = new JObject(
                            new JProperty("res", "1"),
                            new JProperty("msg", "")
                        ).ToString();
            return strResult;
        }
        public string orderlist(HttpContext context)
        {
            string fuserid = context.Request["fuserid"] == null ? "" : context.Request["fuserid"].ToString();//获取参数
            string ftype = context.Request["ftype"] == null ? "" : context.Request["ftype"].ToString();//获取参数
            string fpage = context.Request["fpage"] == null ? "" : context.Request["fpage"].ToString();//获取参数
            string strUrl = System.Configuration.ConfigurationManager.AppSettings["URL_PIC"];
            string sql = string.Format(@"SELECT A.*,'{0}'||A.FGDSEQ||'/'||B.FNAME FURL,to_char(FUPTTIME,'yyyy-MM-dd') FDATE
                        FROM DAT_GOODSORDER A,DOC_GOODSPIC B
                        WHERE A.FGDSEQ = B.FGDSEQ
                        AND B.FTYPE = 'T4' AND A.FUSERID = '{1}'", strUrl, fuserid);

            if (ftype != "全部")
            {
                sql += string.Format(" AND A.FFLAG = '{0}'", ftype);
            }
            int total = 0;
            DataTable dt = PubFunc.DbGetPage(Convert.ToInt16(fpage) - 1, 5, sql + "ORDER BY A.FUPTTIME DESC", ref total);
            string strResult;
            if (dt.Rows.Count < 1)
            {
                if (fpage == "1")
                {
                    strResult = new JObject(
                                    new JProperty("status", "empty"),
                                    new JProperty("data", "")
                                ).ToString();
                }
                else
                {
                    strResult = new JObject(
                                    new JProperty("status", "nomore"),
                                    new JProperty("data", "")
                                ).ToString();
                }
            }
            else
            {
                DataView dv = new DataView(dt);
                DataTable dtData = dv.ToTable(true, "FDATE", "FBILLNO", "FFLAG");
                List<JObject> jObjects = new List<JObject>();
                foreach (DataRow dr in dtData.Rows)
                {
                    List<JObject> jObjectsChild = new List<JObject>();
                    foreach (DataRow drChild in dt.Select("FBILLNO = '" + dr["FBILLNO"] + "' AND FFLAG = '" + dr["FFLAG"] + "'"))
                    {
                        JObject jArrayChild = new JObject(
                        new JProperty("goods_id", drChild["FGDSEQ"]),
                        new JProperty("goods_name", drChild["FGDNAME"]),
                        new JProperty("goods_img", drChild["FURL"]),
                        new JProperty("goods_price", drChild["FHSJJ"]),
                        new JProperty("goods_buynum", drChild["FSL"]));
                        jObjectsChild.Add(jArrayChild);
                    }
                    JObject jArray = new JObject(
                                new JProperty("items", jObjectsChild),
                                new JProperty("orderDate", dr["FDATE"]),
                                new JProperty("orderNumber", dr["FBILLNO"]),
                                new JProperty("status", dr["FFLAG"])
                            );
                    jObjects.Add(jArray);
                }
                strResult = new JObject(
                                    new JProperty("status", "ok"),
                                    new JProperty("data", jObjects)
                                ).ToString();
            }
            return strResult;
        }
    }
}