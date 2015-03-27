using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace FishML {

   
    public partial class Form1 : Form {

        int readMinutesToGo;
        int readHoursToGo;
        int readSecsToGo;

        int writeMinutesToGo;
        int writeHoursToGo;
        int writeSecsToGo;
        System.Windows.Forms.Timer tmrRead;
        System.Windows.Forms.Timer tmrReadCount;
        System.Windows.Forms.Timer tmrWrite;
        System.Windows.Forms.Timer tmrWriteCount;
        public Form1() {
            
            InitializeComponent();
            btn1.Enabled = false;
            btn2.Enabled = false;
            myData = myData.Load();
            BuildConnectionString();
            myTimes = myTimes.Load();
            readHoursToGo = (int) myTimes.ReadEvery / 60;
            if ( readHoursToGo == 0 ) {
                readMinutesToGo = myTimes.ReadEvery;
            }
            tmrReadCount = new System.Windows.Forms.Timer();
            tmrReadCount.Interval = 1000;
            tmrReadCount.Tick += ( s, e ) => {
                readSecsToGo--;
                if ( readSecsToGo < 0 ) {
                    readSecsToGo = 59;
                    readMinutesToGo--;
                    if ( readMinutesToGo < 0 ) {
                        readMinutesToGo = 59;
                        readHoursToGo--;
                        if ( readHoursToGo < 0 ) {
                            readHoursToGo = 0;
                        }
                    }
                }
                lblReadFile.Text = string.Format( "Απομένουν {0} ώρες, {1} λεπτά και {2} δευτερόλεπτά για ανάγνωση αρχείου", readHoursToGo, readMinutesToGo, readSecsToGo );

            };
            tmrReadCount.Start();

            tmrRead = new System.Windows.Forms.Timer();
            tmrRead.Interval =   myTimes.ReadEvery * 60 * 1000;
            tmrRead.Tick += ( s, e ) => {
                TickRead();
            };            
            tmrRead.Start();


            writeHoursToGo = (int)myTimes.WriteEvery / 60;
            if ( writeHoursToGo == 0 ) {
                writeMinutesToGo = myTimes.WriteEvery;
            }
            tmrWriteCount = new System.Windows.Forms.Timer();
            tmrWriteCount.Interval = 1000;
            tmrWriteCount.Tick += ( s, e ) => {
                writeSecsToGo--;
                if ( writeSecsToGo < 0 ) {
                    writeSecsToGo = 59;
                    writeMinutesToGo--;
                    if ( writeMinutesToGo < 0 ) {
                        writeMinutesToGo = 59;
                        writeHoursToGo--;
                        if ( writeHoursToGo < 0 ) {
                            writeHoursToGo = 0;
                        }
                    }
                }
                lblWriteFile.Text = string.Format( "Απομένουν {0} ώρες, {1} λεπτά και {2} δευτερόλεπτά για δημιουργία αρχείου", writeHoursToGo, writeMinutesToGo, writeSecsToGo );

            };
            tmrWriteCount.Start();

            tmrWrite = new System.Windows.Forms.Timer();
            tmrWrite.Interval =  myTimes.WriteEvery  * 60 * 1000;
            tmrWrite.Tick += ( s, e ) => {
                TickWrite();
            };
            tmrWrite.Start();
           
        }

        private void TickWrite() {
            try {
                tmrWriteCount.Stop();
                tmrWrite.Stop();
                lblWriteFile.Text = "Writing File";
                WriteXmlFile();
            } finally {
                writeMinutesToGo = 0;
                writeHoursToGo = (int)myTimes.WriteEvery / 60;
                if ( writeHoursToGo == 0 ) {
                    writeMinutesToGo = myTimes.WriteEvery;
                }
                writeSecsToGo = 0;
                tmrWriteCount.Start();
                tmrWrite.Start();
            }
        }

        private void TickRead() {
            try {
                tmrReadCount.Stop();
                tmrRead.Stop();
                lblReadFile.Text = "Reading File";
                ReadXMLFile( myTimes.DataFolderRead );
            } finally {
                readMinutesToGo = 0;
                readHoursToGo = (int)myTimes.ReadEvery / 60;
                if ( readHoursToGo == 0 ) {
                    readMinutesToGo = myTimes.ReadEvery;
                }
                readSecsToGo = 0;
                tmrRead.Start();
                tmrReadCount.Start();
            }
        }



        private void btn1_Click( object sender, EventArgs e ) {
            try {
                btn1.Enabled = false;
                TickRead();
            } finally {
                tmrRead.Start();
                btn1.Enabled = true;
            }
            
        }
        
        private void WriteXmlFile() {
            string xmlString = string.Empty;
            SqlDataReader rdr = null;
            string commandString = @"Select case when Charindex(';', t.spcode) = 0 then t.spcode else Substring(t.spcode, 1,Charindex(';', t.spcode)-1) end item_id,
case when Charindex(';', t.spcode) = 0 then '' else Substring(t.spcode, Charindex(';', t.spcode)+1, LEN(t.spcode)) end item_subid
, sum(IsNull(k.Qty,0)) item_pos
from Ai_prdct p
join I_Item t on p.idprdct = t.iditem
join i_stock k on k.IDItem = t.IDItem
where t.spcode is not null
group by case when Charindex(';', t.spcode) = 0 then t.spcode else Substring(t.spcode, 1,Charindex(';', t.spcode)-1) end ,
case when Charindex(';', t.spcode) = 0 then '' else Substring(t.spcode, Charindex(';', t.spcode)+1, LEN(t.spcode)) end 
FOR XML RAW ('item'), ROOT ('items'), ELEMENTS;
";
            try {
                // Open the connection
                conSrc.Open();

                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand( commandString, conSrc );

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();

                // print the CategoryName of each record
                while ( rdr.Read() ) {
                    xmlString= rdr[0].ToString();
                   
                }
            } finally {
                // close the reader
                if ( rdr != null ) {
                    rdr.Close();
                }

                // Close the connection
                if ( conSrc != null ) {
                    conSrc.Close();
                }


            }
            using ( FileStream fs = new FileStream( myTimes.DataFolderWrite,
            FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite ) ) {
                XmlDocument xmlDoc = new XmlDocument();
                
                xmlDoc.LoadXml( string.Format( "<?xml version='1.0' ?>{0}", xmlString ) );
                                



                xmlDoc.Save( fs );
            }
        }


        data myData;
        times myTimes;
        List<string> omadesApoArxeio ;
        List<string> catigoriesApoArxeio ; 
        List<string> ypoCatigoriesApoArxeio;
        List<string> promitheftesApoArxeio ;
        List<string> monadesMetrisisApoArxeio;
        List<string> eidiApoArxeio;
        List<string> barcodeApoArxeio;
        Dictionary<int, string> eidiSeTimokatalogoLianikis = new Dictionary<int, string>();
        Dictionary<int, string> eidiSeTimokatalogoXondrikis = new Dictionary<int, string>();

        Dictionary<string, decimal> eidiMeTimesXondrikis = new Dictionary<string, decimal>();
        Dictionary<string, decimal> eidiMeTimesLiankis = new Dictionary<string, decimal>();
        private void button1_Click( object sender, EventArgs e ) {
            DialogResult result = openFD.ShowDialog(); // Show the dialog.
            if ( result == DialogResult.OK ) {
                string file = openFD.FileName;
                UseWaitCursor = true;
                textBox1.Text = file;
                try {
                    Application.DoEvents();
                    //Thread.Sleep( 2000 );
                    lblReadFile.Text = "Ανάγνωση αρχείου " + file;                    
                    WaitReady( file );
                    lblReadFile.Text = string.Format( "Reading file {0}. Please wait...", file );
                    ReadXMLFile( file );
                    SetControlPropertyThreadSafe( lblReadFile, "Text", "Εισαγωγή εγγραφών (είδη - πελάτες - παραστατικά)" );
                } catch ( IOException ) {
                } finally {
                    UseWaitCursor = false;
                }
            }
        }

        private void ReadXMLFile( string file ) {
            try {
                Application.DoEvents();
                DataSet dset = new DataSet();
                dset.ReadXml( file );

                omadesApoArxeio = dset.Tables[0].Rows.OfType<DataRow>().GroupBy( x => x.Field<string>( "item_omada" ) ).Select( a => a.First().Field<string>( "item_omada" ) ).ToList();
                catigoriesApoArxeio = dset.Tables[0].Rows.OfType<DataRow>().GroupBy( x => x.Field<string>( "item_category1" ) ).Select( a => a.First().Field<string>( "item_category1" ) ).ToList();
                ypoCatigoriesApoArxeio = dset.Tables[0].Rows.OfType<DataRow>().GroupBy( x => x.Field<string>( "item_category2" ) ).Select( a => a.First().Field<string>( "item_category2" ) ).ToList();
                promitheftesApoArxeio = dset.Tables[0].Rows.OfType<DataRow>().GroupBy( x => x.Field<string>( "item_supplier" ) ).Select( a => a.First().Field<string>( "item_supplier" ) ).ToList();
                monadesMetrisisApoArxeio = dset.Tables[0].Rows.OfType<DataRow>().GroupBy( x => x.Field<string>( "item_unit" ) ).Select( a => a.First().Field<string>( "item_unit" ) ).ToList();
                eidiApoArxeio = dset.Tables[0].Rows.OfType<DataRow>().GroupBy( x => x.Field<string>( "item_code" ) ).Select( a => a.First().Field<string>( "item_code" ) ).ToList();
                barcodeApoArxeio = dset.Tables[0].Rows.OfType<DataRow>().GroupBy( x => x.Field<string>( "item_barcode" ) ).Select( a => a.First().Field<string>( "item_barcode" ) ).ToList();

                FillSaved();
                InsertOther( dset.Tables[0].Rows );

                foreach ( DataRow dRow in dset.Tables[0].Rows ) {
                    readingRow = dRow;
                    if ( eidi.ContainsKey( dRow["item_code"].ToString() ) ) {
                        UpdateItem();
                    } else {
                        insertItems();
                    }
                    if ( dRow["item_netprice"].ToString() != string.Empty ) {
                        eidiMeTimesXondrikis[ dRow["item_code"].ToString()]= Convert.ToDecimal( dRow["item_netprice"] ) ;
                    }
                    if ( dRow["item_retailprice"].ToString() != string.Empty ) {
                        eidiMeTimesLiankis[ dRow["item_code"].ToString()]= Convert.ToDecimal( dRow["item_retailprice"] ) ;
                    }
                    Application.DoEvents();
                }
                insertItemsCmd();

                DiavasmaEidwnPouYparxounStonAI_Prdct();

                InsertBarcode( dset.Tables[0].Rows );

                DoTimokatalogous();
            } catch {
                int a = 0;
            } finally {

               // MessageBox.Show( "ΤΕΛΟΣ" );
            }
        }


        #region timokatalogoi
        private void DoTimokatalogous() {
            Application.DoEvents();
            GetEidiPouYparxounSeTimokatalogous();
            int IDRetail = -1;
            int IDWholes = -1;
            DateTime dtFromRetail = DateTime.MinValue;
            DateTime dtFromWholes = DateTime.MinValue;

            SqlDataReader rdr = null;

            try {
                // Open the connection
                conSrc.Open();

                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand( @"select d.IDList, d.dtfrom from SBP_LstDt d join SBP_Gprm g on d.IDList = g.IDRetailList
where GetDate() between d.Dtfrom and Isnull(d.dtto,getdate())
", conSrc );

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();

                // print the CategoryName of each record
                while ( rdr.Read() ) {
                    IDRetail = Convert.ToInt32( rdr[0] );
                    dtFromRetail = Convert.ToDateTime( rdr[1] );
                }
            } finally {
                // close the reader
                if ( rdr != null ) {
                    rdr.Close();
                }

                // Close the connection
                if ( conSrc != null ) {
                    conSrc.Close();
                }
            }

            rdr = null;

            try {
                // Open the connection
                conSrc.Open();

                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand( @"select d.IDList, d.dtfrom from SBP_LstDt d join SBP_Gprm g on d.IDList = g.IDWholesaleList
where GetDate() between d.Dtfrom and Isnull(d.dtto,getdate())
", conSrc );

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();

                // print the CategoryName of each record
                while ( rdr.Read() ) {
                    IDWholes = Convert.ToInt32( rdr[0] );
                    dtFromWholes = Convert.ToDateTime( rdr[1] );
                }
            } finally {
                // close the reader
                if ( rdr != null ) {
                    rdr.Close();
                }

                // Close the connection
                if ( conSrc != null ) {
                    conSrc.Close();
                }

            }
            Application.DoEvents();
            List<int> idpassed = new List<int>();
            foreach ( var item in eidi.Where( a => eidiSeTimokatalogoLianikis.ContainsKey( a.Value ) ) ) {               
                UpdateList( IDRetail, dtFromRetail, item.Key, eidiMeTimesLiankis );
                Application.DoEvents();
            }
            foreach ( var item in eidi.Where( a => !eidiSeTimokatalogoLianikis.ContainsKey( a.Value ) ) ) {
                if ( idpassed.Contains( item.Value ) ) {
                    continue;
                }
                idpassed.Add( item.Value );
                InsertList( IDRetail, dtFromRetail, item.Key, eidiMeTimesLiankis );
                Application.DoEvents();
            }
            foreach ( var item in eidi.Where( a => eidiSeTimokatalogoXondrikis.ContainsKey( a.Value ) ) ) {
                UpdateList( IDWholes, dtFromWholes, item.Key, eidiMeTimesXondrikis );
                Application.DoEvents();
            }
            idpassed = new List<int>();
            foreach ( var item in eidi.Where( a => !eidiSeTimokatalogoXondrikis.ContainsKey( a.Value ) ) ) {
                if ( idpassed.Contains( item.Value ) ) {
                    continue;
                }
                idpassed.Add( item.Value );
                InsertList( IDWholes, dtFromWholes, item.Key, eidiMeTimesXondrikis );
                Application.DoEvents();
            }

        }
        
        private void UpdateList(int idList, DateTime dtfrom, string cd, Dictionary<string, decimal> amountsOfEidi){
            string updString = @"update sbp_item set amnt=@amnt where idlist=@idlist and dtfrom=@dtfrom and idprdct=(select idprdct from ai_prdct where cd=@cd)";
            using ( SqlCommand cmd = new SqlCommand( updString, conSrc ) ) {

                cmd.Parameters.AddWithValue( "@idlist", idList );
                cmd.Parameters.AddWithValue( "@dtfrom", dtfrom );
                cmd.Parameters.AddWithValue( "@cd", cd );
                cmd.Parameters.AddWithValue( "@amnt", amountsOfEidi[cd] );

                conSrc.Open();
                cmd.ExecuteNonQuery();
                if ( conSrc.State == System.Data.ConnectionState.Open )
                    conSrc.Close();
            }
        }

        private void InsertList( int idList, DateTime dtfrom, string cd, Dictionary<string, decimal> amountsOfEidi ) {
            string updString = @"insert into sbp_item (amnt,idlist,dtfrom,idprdct) select @amnt, @idlist,@dtfrom, idprdct from ai_prdct where cd=@cd";
            using ( SqlCommand cmd = new SqlCommand( updString, conSrc ) ) {

                cmd.Parameters.AddWithValue( "@idlist", idList );
                cmd.Parameters.AddWithValue( "@dtfrom", dtfrom );
                cmd.Parameters.AddWithValue( "@cd", cd );
                cmd.Parameters.AddWithValue( "@amnt", amountsOfEidi[cd] );

                conSrc.Open();
                cmd.ExecuteNonQuery();
                if ( conSrc.State == System.Data.ConnectionState.Open )
                    conSrc.Close();
            }
        }
        #endregion

        #region updates
        private void UpdateItem() {
            string cd = readingRow["item_code"].ToString();
            string dscr = readingRow["item_title"].ToString() ;
            string custom1 = readingRow["item_description"].ToString();
            string custom2 = readingRow["item_titleprint"].ToString();
            int idgrp = omades[readingRow["item_omada"].ToString()];
            int idcat = catigories[readingRow["item_category1"].ToString()];
            int idcatsub = ipocatigories[readingRow["item_category2"].ToString()];
            int idsplr = readingRow["item_supplier"].ToString() == string.Empty ? -1 : promitheftes.ContainsKey( readingRow["item_supplier"].ToString() ) ? promitheftes[readingRow["item_supplier"].ToString()] : -1;
            int idmsr = monadmetrisis[readingRow["item_unit"].ToString()];

            string updString = @"update ai_prdct set idgrp = @idgrp, idmainsplr=@idsplr, idmsr=@idmsr, dscr=@dscr, customdscr1=@custom1, customdscr2=@custom2 where cd=@cd";
            UpdateData( updString, new object[] { idgrp, idsplr, idmsr, dscr, custom1, custom2, cd }, true );

            updString = @"update i_item set idcat = @idcat, idcatSub = @idcatsub from i_item t join ai_prdct p on t.iditem = p.idprdct where p.cd=@cd";
            UpdateData( updString, new object[] { idcat, idcatsub, cd }, false );
        }

        public void UpdateData( string updString, object[] paramets , bool forAI_Prdct) {

            using ( SqlCommand cmd = new SqlCommand( updString, conSrc ) ) {
                if ( forAI_Prdct ) {

                    cmd.Parameters.AddWithValue( "@idgrp", paramets[0] );
                    if ( paramets[1].ToString() == "-1" ) {
                        paramets[1] = DBNull.Value;
                    }
                    cmd.Parameters.AddWithValue( "@idsplr", paramets[1] );
                    cmd.Parameters.AddWithValue( "@idmsr", paramets[2] );
                    cmd.Parameters.AddWithValue( "@dscr", paramets[3] );
                    cmd.Parameters.AddWithValue( "@custom1", paramets[4].ToString().Substring( 0, Math.Min( 63, paramets[4].ToString().Length ) ) );
                    cmd.Parameters.AddWithValue( "@custom2", paramets[5].ToString().Substring( 0, Math.Min( 63, paramets[5].ToString().Length ) ) );
                    cmd.Parameters.AddWithValue( "@cd", paramets[6] );
                } else {
                    cmd.Parameters.AddWithValue( "@idcat", paramets[0] );
                    cmd.Parameters.AddWithValue( "@idcatsub", paramets[1] );
                    cmd.Parameters.AddWithValue( "@cd", paramets[2] );
                }
                conSrc.Open();
                cmd.ExecuteNonQuery();
                if ( conSrc.State == System.Data.ConnectionState.Open )
                    conSrc.Close();
               
            }
        }
        #endregion

        #region insert new rows in lookup tables
        private void InsertOther(DataRowCollection rows) {
            foreach ( var val in omadesApoArxeio.Where( a => !omades.ContainsKey( a ) ) ) {
                string code = DateTime.Now.Ticks.ToString().Substring(0,15);
                omades[val] = Insertdata( " INSERT INTO AIO_Grp (Dscr, cd, MustExportTrias ) VALUES ( @Dscr, @cd, 1 ) ; SELECT IDGrp from aio_Grp where idgrp = CAST(scope_identity() AS int);", new object[] { val, code } );
                Application.DoEvents();
            }
            foreach ( var val in catigoriesApoArxeio.Where( a => !catigories.ContainsKey( a ) ) ) {
                string code = DateTime.Now.Ticks.ToString().Substring( 0, 15 );
                catigories[val] = Insertdata( " INSERT INTO AIO_CAT (Dscr, cd, MustExportTrias ) VALUES ( @Dscr, @cd, 1 ) ; SELECT IDCat from AIO_CAT where IDCat = CAST(scope_identity() AS int);", new object[] { val, code } );
                Application.DoEvents();
            }

            foreach ( var val in ypoCatigoriesApoArxeio.Where( a => !ipocatigories.ContainsKey( a ) ) ) {
                string code = DateTime.Now.Ticks.ToString().Substring( 0, 16 );
                string catCode = rows.OfType<DataRow>().Where( a => a["item_category2"].ToString() == val ).First()["item_category1"].ToString();
                int catid = catigories[catCode];
                ipocatigories[val] = Insertdata( " INSERT INTO AIO_CatSub ( DSCR, Cd, IDCat ) VALUES ( @Dscr, @cd, @IDCat ) ; SELECT IDCatSub from AIO_CatSub where IDCatSub = CAST(scope_identity() AS int);", new object[] { val, code, catid } );
                Application.DoEvents();
            }

           foreach ( var val in monadesMetrisisApoArxeio.Where( a => !monadmetrisis.ContainsKey( a ) ) ) {
                string code = DateTime.Now.Ticks.ToString().Substring( 0, 15 );
                monadmetrisis[val] = Insertdata( "INSERT INTO I_Msr (cd, dscr, Rnd, RndMth, Fmt, Sep, ExportVersion ) VALUES ( @cd, @Dscr, 3, 1, '3', ',', -1 ) ; SELECT IDMsr from I_Msr where IDMsr = CAST(scope_identity() AS int);", new object[] { val, code } );
                Application.DoEvents();
           }

        }
        public int Insertdata( string insertString, object[] paramets ) {

            using ( SqlCommand cmd = new SqlCommand( insertString, conSrc ) ) {

                cmd.Parameters.AddWithValue( "@Dscr", paramets[0] );
                cmd.Parameters.AddWithValue( "@cd", paramets[1] );
                if ( paramets.Count() == 3 ) {
                    cmd.Parameters.AddWithValue( "@IDCat", paramets[2] );
                }
                conSrc.Open();
                var modified = cmd.ExecuteScalar();
                if ( conSrc.State == System.Data.ConnectionState.Open )
                    conSrc.Close();
                return Convert.ToInt32( modified );

            }
        }

        private void InsertBarcode( DataRowCollection rows ) {
            foreach ( var val in barcodeApoArxeio.Where( a => !barcodes.ContainsKey( a ) ) ) {
                string cdPrdct = rows.OfType<DataRow>().Where( a => a["item_barcode"].ToString() == val ).First()["item_code"].ToString();
                barcodes[val] = InsertdataBarcode( @"
INSERT INTO AI_Barcode (Dscr, barcode, IDPrdct, IDBarcode, IDMsr) 
select p.dscr, @barcode, p.idprdct , p.IDBarcode, p.IDMsr
from AI_Prdct p where cd = @cd
; update AI_Prdct set IDBarcode = IDBarcode +1 where cd = @cd

 ; SELECT IDPrdct, IDBarcode from AI_Barcode where barcode = @barcode;", new object[] { val, cdPrdct } );
                Application.DoEvents();
            }
        } 

        public string InsertdataBarcode( string insertString, object[] paramets ) {

            using ( SqlCommand cmd = new SqlCommand( insertString, conSrc ) ) {

                cmd.Parameters.AddWithValue( "@barcode", paramets[0] );
                cmd.Parameters.AddWithValue( "@cd", paramets[1] );        
                conSrc.Open();
                var modified = cmd.ExecuteScalar();
                if ( conSrc.State == System.Data.ConnectionState.Open )
                    conSrc.Close();
                int[] modifArray = modified as int[];
                if ( modifArray != null ) {
                    return modifArray[0].ToString() + ";" + modifArray[1].ToString();
                }
                return string.Empty;

            }
        }


        #endregion

        protected override void OnLoad( EventArgs e ) {
            base.OnLoad( e );
            if ( !TestConnection( strSrc ) ) {
                MessageBox.Show( "Wrong connection Settings" );
                Close();
            }
           // WatchAFolder( myData.path );
        }

        #region File/Folder related
        private void WatchAFolder( string path ) {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.xml";
            watcher.Created += OnCreated;
            watcher.EnableRaisingEvents = true;
        }

        DataRow readingRow;
        private void OnCreated( object sender, FileSystemEventArgs e ) {
            if ( e.ChangeType == WatcherChangeTypes.Created ) {
                // c# read xml file     
                WaitReady( e.FullPath );               
                ReadXMLFile( e.FullPath );
            }
        }

        public static void WaitReady( string fileName ) {
            while ( true ) {
                try {
                    using ( Stream stream = System.IO.File.Open( fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite ) ) {
                        if ( stream != null ) {
                            System.Diagnostics.Trace.WriteLine( string.Format( "Output file {0} ready.", fileName ) );
                            break;
                        }
                    }
                } catch ( FileNotFoundException ex ) {
                    System.Diagnostics.Trace.WriteLine( string.Format( "Output file {0} not yet ready ({1})", fileName, ex.Message ) );
                } catch ( IOException ex ) {
                    System.Diagnostics.Trace.WriteLine( string.Format( "Output file {0} not yet ready ({1})", fileName, ex.Message ) );
                } catch ( UnauthorizedAccessException ex ) {
                    System.Diagnostics.Trace.WriteLine( string.Format( "Output file {0} not yet ready ({1})", fileName, ex.Message ) );
                }
                Thread.Sleep( 500 );
            }
        }

        #endregion

        
        Dictionary<string, int> omades = new Dictionary<string, int>();
        Dictionary<string, int> catigories = new Dictionary<string, int>();
        Dictionary<string, int> ipocatigories = new Dictionary<string, int>();
        Dictionary<string, int> monadmetrisis = new Dictionary<string, int>();
        Dictionary<string, int> promitheftes = new Dictionary<string, int>();
        Dictionary<string, int> eidi = new Dictionary<string, int>();
        Dictionary<string, string> barcodes = new Dictionary<string, string>();

        #region Fill already inserted values

        /// <summary>
        /// Fill already saved tables in each Dictionary
        /// </summary>
        private void FillSaved() {
            foreach ( var val in omadesApoArxeio ) {
                Fill( string.Format("select IDGrp from AIO_Grp where dscr = '{0}'", val), 2, val );
                Application.DoEvents();
            }
            foreach ( var val in catigoriesApoArxeio ) {
                Fill( string.Format( "select IDCat from AIO_Cat where dscr = '{0}'", val ), 3, val );
                Application.DoEvents();
            }
            foreach ( var val in ypoCatigoriesApoArxeio ) {
                Fill( string.Format( "select IDCatSub from AIO_CatSub where dscr = '{0}'", val ), 4, val );
                Application.DoEvents();
            }
            foreach ( var val in promitheftesApoArxeio ) {
                Fill( string.Format( "select IDParty from AP_Party where dscr = '{0}' and Bclass = 1", val ), 5, val );
                Application.DoEvents();
            }
            foreach ( var val in monadesMetrisisApoArxeio ) {
                Fill( string.Format( "select IDMsr from I_Msr where dscr = '{0}'", val ), 6, val );
                Application.DoEvents();
            }
            DiavasmaEidwnPouYparxounStonAI_Prdct();

            foreach ( var val in barcodeApoArxeio ) {
                if ( val != string.Empty ) {
                    Fill( string.Format( "select IDPrdct, IDBarcode from AI_Barcode where barcode = '{0}'", val ), 8, val );
                }
                Application.DoEvents();
            }


        }

        private void DiavasmaEidwnPouYparxounStonAI_Prdct() {
            foreach ( var val in eidiApoArxeio ) {
                Fill( string.Format( "select IDPrdct from AI_Prdct where cd = '{0}'", val ), 7, val );
                Application.DoEvents();
            }
        }

        private void Fill(string commandString, int type, string val) {
            SqlDataReader rdr = null;
          
            try {
                // Open the connection
                conSrc.Open();

                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand( commandString, conSrc );

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();

                // print the CategoryName of each record
                while ( rdr.Read() ) {
                    //Console.WriteLine( rdr[0] );
                    switch ( type ) {
                        case 2:  //omada
                            omades[val] = Convert.ToInt32(rdr[0]) ;
                            break;
                        case 3: //catigoria
                            catigories[val] = Convert.ToInt32( rdr[0] );
                            break;
                        case 4: //ipokatigoria
                            ipocatigories[val] = Convert.ToInt32( rdr[0] );
                            break;
                        case 5: //promitheftis
                            promitheftes[val] = Convert.ToInt32( rdr[0] );
                            break;
                        case 6: //monades metrisis
                            monadmetrisis[val] = Convert.ToInt32( rdr[0] );
                            break;
                        case 7: //eidi
                            eidi[val] = Convert.ToInt32( rdr[0] );
                            break;
                        case 8: //barcodes
                            barcodes[val] = rdr[0].ToString() + ";" + rdr[1].ToString();
                            break;
                    }
                }
            } finally {
                // close the reader
                if ( rdr != null ) {
                    rdr.Close();
                }

                // Close the connection
                if ( conSrc != null ) {
                    conSrc.Close();
                }


            }
        }



        private void GetEidiPouYparxounSeTimokatalogous() {
            string commandString = "select p.cd, p.IDPrdct from SBP_Item t join SBP_Gprm g on t.IDList = g.IDRetailList join AI_Prdct p on p.IDPrdct = t.IDPrdct";

            SqlDataReader rdr = null;

            try {
                // Open the connection
                conSrc.Open();

                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand( commandString, conSrc );

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();

                // print the CategoryName of each record
                while ( rdr.Read() ) {
                    eidiSeTimokatalogoLianikis.Add( Convert.ToInt32(rdr[1]), Convert.ToString( rdr[0] ) );
                }
            } finally {
                // close the reader
                if ( rdr != null ) {
                    rdr.Close();
                }

                // Close the connection
                if ( conSrc != null ) {
                    conSrc.Close();
                }


            }

            commandString = "select p.cd, p.IDPrdct from SBP_Item t join SBP_Gprm g on t.IDList = g.IDWholeSaleList join AI_Prdct p on p.IDPrdct = t.IDPrdct";

            rdr = null;

            try {
                // Open the connection
                conSrc.Open();

                // 1. Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand( commandString, conSrc );

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();

                // print the CategoryName of each record
                while ( rdr.Read() ) {
                    eidiSeTimokatalogoXondrikis.Add( Convert.ToInt32( rdr[1] ), Convert.ToString( rdr[0] ) );
                }
            } finally {
                // close the reader
                if ( rdr != null ) {
                    rdr.Close();
                }
                // Close the connection
                if ( conSrc != null ) {
                    conSrc.Close();
                }
            }
        }

        
        #endregion


        #region insert items
        private void insertItems() {
            //items

            // execute an insert query using c# sql server
            string stmt = @"insert into I_ItemPort
                        ( 
                        recnum, 
                        buy, 
                        cd, 
                        Clng, 
                        ClngOrdr, 
                        Cmnt,
                        Conv, 
                        Discount,
                        Dscr,
                        IDCat, 
                     --   IDCatSub,
                        IDFml, 
                        IDGrp,
                       -- IDGrpSub,
                        IDILedg, 
                        IDMsr, 
                        IDVCls,
                        imported, 
                        LotCls, 
                        PrdctKind, 
                        Sale,
                        SNCls, 
                        spCode, 
                        Sts 
                        ) 
select 
                        
                        @recnum, 
                        @buy,
                        @cd, 
                        @Clng, 
                        @ClngOrdr, 
                        @Cmnt, 
                        @Conv, 
                        @Discount,
                        @Dscr, 
                        @IDCat,
                        --@IDCatSub,
                        @IDFml, 
                        @IDGrp, 
                        --@IDGrpSub,
                        @IDILedg, 
                        @IDMsr, 
                        @IDVCls, 
                        @imported,
                        @LotCls,
                        @PrdctKind, 
                        @Sale, 
                        @SNCls, 
                        @spCode,
                        @Sts 
                       

where not exists (select 1 from I_ItemPort where cd = @cd)
";
            SqlCommand cmd = new SqlCommand( stmt, conSrc );
            cmd.Parameters.Add( "@recnum", SqlDbType.VarChar, 64 );
            cmd.Parameters.Add( "@Buy", SqlDbType.Int );
            cmd.Parameters.Add( "@cd", SqlDbType.VarChar, 32 );
            cmd.Parameters.Add( "@Clng", SqlDbType.Decimal );
            cmd.Parameters.Add( "@ClngOrdr", SqlDbType.Decimal );
            cmd.Parameters.Add( "@Cmnt", SqlDbType.VarChar, 1200 );
            cmd.Parameters.Add( "@Conv", SqlDbType.Decimal );
            cmd.Parameters.Add( "@Discount", SqlDbType.Decimal );
            cmd.Parameters.Add( "@Dscr", SqlDbType.VarChar, 128 );
            cmd.Parameters.Add( "@IDCat", SqlDbType.Int );
            cmd.Parameters.Add( "@IDCatSub", SqlDbType.Int );
            cmd.Parameters.Add( "@IDFml", SqlDbType.Int );
            cmd.Parameters.Add( "@IDGrp", SqlDbType.Int );
            cmd.Parameters.Add( "@IDMainSplr", SqlDbType.Int );
            cmd.Parameters.Add( "@IDILedg", SqlDbType.Int );
            cmd.Parameters.Add( "@IDMsr", SqlDbType.Int );
            cmd.Parameters.Add( "@IDVCls", SqlDbType.Int );
            cmd.Parameters.Add( "@imported", SqlDbType.Int );
            cmd.Parameters.Add( "@LotCls", SqlDbType.Int );
            cmd.Parameters.Add( "@PrdctKind", SqlDbType.Int );
            cmd.Parameters.Add( "@Sale", SqlDbType.Int );
            cmd.Parameters.Add( "@SNCls", SqlDbType.Int );
            cmd.Parameters.Add( "@spCode", SqlDbType.VarChar, 16 );
            cmd.Parameters.Add( "@Sts", SqlDbType.Int );

            cmd.Parameters["@recnum"].Value = readingRow["item_code"].ToString();
            cmd.Parameters["@Buy"].Value = myData.prdctdefaults.buy;
            cmd.Parameters["@cd"].Value = readingRow["item_code"].ToString();
            cmd.Parameters["@Clng"].Value = myData.prdctdefaults.Clng;
            cmd.Parameters["@ClngOrdr"].Value = myData.prdctdefaults.ClngOrder;
            cmd.Parameters["@Cmnt"].Value = myData.prdctdefaults.Cmnt;
            cmd.Parameters["@Conv"].Value = myData.prdctdefaults.Conv;
            cmd.Parameters["@Discount"].Value = myData.prdctdefaults.Discount;
            cmd.Parameters["@Dscr"].Value = readingRow["item_title"].ToString();
            cmd.Parameters["@IDCat"].Value = catigories[readingRow["item_category1"].ToString()];
            cmd.Parameters["@IDCatSub"].Value = ipocatigories[readingRow["item_category2"].ToString()];
            cmd.Parameters["@IDFml"].Value = myData.prdctdefaults.IDFml;
            cmd.Parameters["@IDGrp"].Value = omades[readingRow["item_omada"].ToString()];

            object splr = DBNull.Value;
            if ( readingRow["item_supplier"].ToString() == string.Empty ) {
                splr = DBNull.Value;
            } else if (promitheftes.ContainsKey( readingRow["item_supplier"].ToString())){
                splr = promitheftes[readingRow["item_supplier"].ToString()];
            } else {
                 splr = DBNull.Value;
            }
            cmd.Parameters["@IDMainSplr"].Value = splr;
            cmd.Parameters["@IDILedg"].Value = myData.prdctdefaults.IDILedg;
            cmd.Parameters["@IDMsr"].Value = monadmetrisis[readingRow["item_unit"].ToString()];
            cmd.Parameters["@IDVCls"].Value = myData.vclsData.Where( a => a.from == readingRow["item_taxpercentage"].ToString() ).First().to;
            cmd.Parameters["@imported"].Value = 0;
            cmd.Parameters["@LotCls"].Value = myData.prdctdefaults.LotCls;
            cmd.Parameters["@PrdctKind"].Value = myData.prdctdefaults.PrdctKind;
            cmd.Parameters["@Sale"].Value = myData.prdctdefaults.Sale;
            cmd.Parameters["@SNCls"].Value = myData.prdctdefaults.SNCls;
            cmd.Parameters["@spCode"].Value = readingRow["item_id"].ToString() +(readingRow["item_subid"].ToString() == string.Empty ? string.Empty : ";" + readingRow["item_subid"].ToString());
            cmd.Parameters["@Sts"].Value = 1;

            execCommand( strSrc, cmd );



        }



        private void insertItemsCmd() {
            System.Diagnostics.Process p1 = null;
            try {
                string targetDir;
                Environment.CurrentDirectory = Application.StartupPath;
                targetDir = Environment.CurrentDirectory;
                p1 = new System.Diagnostics.Process();
                p1.StartInfo.WorkingDirectory = targetDir;
                p1.StartInfo.FileName = @"EpsilonCmdI_ItemGTI.cmd";
                p1.StartInfo.CreateNoWindow = true;                
                p1.EnableRaisingEvents = true;
                p1.Start();

            } catch ( Exception ex ) {
                Console.WriteLine( "Exception Occurred :{0},{1}",
                    ex.Message, ex.StackTrace.ToString() );
            }
        }

        #endregion

        #region thread related
        private delegate void SetControlPropertyThreadSafeDelegate( Control control, string propertyName, object propertyValue );

        public static void SetControlPropertyThreadSafe( Control control, string propertyName, object propertyValue ) {
            if ( control.InvokeRequired ) {
                control.Invoke( new SetControlPropertyThreadSafeDelegate( SetControlPropertyThreadSafe ), new object[] { control, propertyName, propertyValue } );
            } else {
                control.GetType().InvokeMember( propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue } );
            }
        }

        #endregion


        public SqlConnection conSrc = new SqlConnection();
        public string strSrc = "";

        public void BuildConnectionString() {
            strSrc = "Data Source=" + myData.servername + ";Initial Catalog=" + myData.databasename + ";User Id=" + myData.username + ";Password=" + myData.password;
            conSrc.ConnectionString = strSrc;
        }

        public bool TestConnection( string ConnectionString ) {
            SqlConnection con = new SqlConnection( ConnectionString );
            try {
                con.Open();
                con.Close();

                return true;
            } catch ( Exception ) {
                return false;
            }

        }



        private void execCommand( string connectionString, SqlCommand dbCommand ) {
            using ( SqlConnection dbConn = new SqlConnection( connectionString ) ) {
                dbConn.Open();
                using ( SqlTransaction dbTrans = dbConn.BeginTransaction() ) {
                    try {
                        dbCommand.Connection = dbConn;
                        dbCommand.Transaction = dbTrans;

                        dbCommand.ExecuteNonQuery();
                        dbTrans.Commit();

                    } catch ( SqlException ) {
                        dbTrans.Rollback();
                        //throw;
                    }
                }
                dbConn.Close();
            }
        }

        public Image SetImageOpacity( Image image, float opacity ) {
            Bitmap bmp = new Bitmap( image.Width, image.Height );
            using ( Graphics g = Graphics.FromImage( bmp ) ) {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = opacity;
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix( matrix, ColorMatrixFlag.Default,
                                                  ColorAdjustType.Bitmap );
                g.DrawImage( image, new Rectangle( 0, 0, bmp.Width, bmp.Height ),
                                   0, 0, image.Width, image.Height,
                                   GraphicsUnit.Pixel, attributes );
            }
            return bmp;
        }
        private void Form1_Load( object sender, EventArgs e ) {
            
            BackgroundImage = SetImageOpacity( global::FishML.Properties.Resources._002, 0.25F );
            btn1.Enabled = true;
            btn2.Enabled = true;
        }

        private void btn2_Click( object sender, EventArgs e ) {
            TickWrite();
            
            tmrWrite.Start();
        }

        private void Form1_Resize( object sender, EventArgs e ) {
            if ( FormWindowState.Minimized == this.WindowState ) {
                mynotifyicon.Visible = true;
                mynotifyicon.ShowBalloonTip( 500 );

                this.Visible = false;
                this.ShowInTaskbar = false;
            } else if ( FormWindowState.Normal == this.WindowState ) {
                mynotifyicon.Visible = false;
            }
        }

        private void mynotifyicon_DoubleClick( object sender, EventArgs e ) {
            if ( FormWindowState.Minimized == this.WindowState ) {
                this.WindowState = FormWindowState.Normal;
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                mynotifyicon.Visible = false;
            }

        }

    }

}
