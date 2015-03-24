using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FishML {

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
    [System.Xml.Serialization.XmlRootAttribute( Namespace = "", IsNullable = false )]
    public partial class data {

        private dataPrdctdefaults prdctdefaultsField;

        private dataVcls[] vclsDataField;

        private string pathField;

        private string servernameField;

        private string databasenameField;

        private string usernameField;

        private string passwordField;

        /// <remarks/>
        public dataPrdctdefaults prdctdefaults {
            get {
                return this.prdctdefaultsField;
            }
            set {
                this.prdctdefaultsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute( "vcls", IsNullable = false )]
        public dataVcls[] vclsData {
            get {
                return this.vclsDataField;
            }
            set {
                this.vclsDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string path {
            get {
                return this.pathField;
            }
            set {
                this.pathField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string servername {
            get {
                return this.servernameField;
            }
            set {
                this.servernameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string databasename {
            get {
                return this.databasenameField;
            }
            set {
                this.databasenameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string username {
            get {
                return this.usernameField;
            }
            set {
                this.usernameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
    }


    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
    public partial class dataPrdctdefaults {

        private byte buyField;

        private decimal clngField;

        private decimal clngOrderField;

        private string cmntField;

        private decimal convField;

        private decimal discountField;

        private byte iDCatField;

        private string iDCatSubField;

        private ushort iDFmlField;

        private byte iDGrpField;

        private string iDGrpSubField;

        private byte iDILedgField;

        private byte iDMsrField;

        private byte iDVClsField;

        private byte lotClsField;

        private byte prdctKindField;

        private byte saleField;

        private byte sNClsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte buy {
            get {
                return this.buyField;
            }
            set {
                this.buyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Clng {
            get {
                return this.clngField;
            }
            set {
                this.clngField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal ClngOrder {
            get {
                return this.clngOrderField;
            }
            set {
                this.clngOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Cmnt {
            get {
                return this.cmntField;
            }
            set {
                this.cmntField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Conv {
            get {
                return this.convField;
            }
            set {
                this.convField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Discount {
            get {
                return this.discountField;
            }
            set {
                this.discountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte IDCat {
            get {
                return this.iDCatField;
            }
            set {
                this.iDCatField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IDCatSub {
            get {
                return this.iDCatSubField;
            }
            set {
                this.iDCatSubField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort IDFml {
            get {
                return this.iDFmlField;
            }
            set {
                this.iDFmlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte IDGrp {
            get {
                return this.iDGrpField;
            }
            set {
                this.iDGrpField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IDGrpSub {
            get {
                return this.iDGrpSubField;
            }
            set {
                this.iDGrpSubField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte IDILedg {
            get {
                return this.iDILedgField;
            }
            set {
                this.iDILedgField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte IDMsr {
            get {
                return this.iDMsrField;
            }
            set {
                this.iDMsrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte IDVCls {
            get {
                return this.iDVClsField;
            }
            set {
                this.iDVClsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte LotCls {
            get {
                return this.lotClsField;
            }
            set {
                this.lotClsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte PrdctKind {
            get {
                return this.prdctKindField;
            }
            set {
                this.prdctKindField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Sale {
            get {
                return this.saleField;
            }
            set {
                this.saleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte SNCls {
            get {
                return this.sNClsField;
            }
            set {
                this.sNClsField = value;
            }
        }
    }


    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
    public partial class dataVcls {

        private string fromField;

        private string toField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string from {
            get {
                return this.fromField;
            }
            set {
                this.fromField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string to {
            get {
                return this.toField;
            }
            set {
                this.toField = value;
            }
        }
    }



    public static class dataExtensions {

        public static void Save( this data Data ) {
            XmlSerializer serializer = new XmlSerializer( Data.GetType() );
            StreamWriter writer = new StreamWriter( "info.xml" );
            serializer.Serialize( writer.BaseStream, Data );
            writer.Dispose();
        }

        public static data Load( this data Data ) {
            XmlSerializer serializer =
              new XmlSerializer( typeof( data ) );
            using ( Stream reader = new FileStream( "info.xml", FileMode.Open ) ) {
                Data = (data)serializer.Deserialize( reader );
            }
            return Data;
        }

    }


    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
    [System.Xml.Serialization.XmlRootAttribute( Namespace = "", IsNullable = false )]
    public partial class times {

        private int writeEveryField;

        private int readEveryField;

        private string dataFolderWriteField;

        private string dataFolderReadField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int WriteEvery {
            get {
                return this.writeEveryField;
            }
            set {
                this.writeEveryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int ReadEvery {
            get {
                return this.readEveryField;
            }
            set {
                this.readEveryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DataFolderWrite {
            get {
                return this.dataFolderWriteField;
            }
            set {
                this.dataFolderWriteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DataFolderRead {
            get {
                return this.dataFolderReadField;
            }
            set {
                this.dataFolderReadField = value;
            }
        }
    }



    public static class timeExtensions {

        public static void Save( this times Times ) {
            XmlSerializer serializer = new XmlSerializer( Times.GetType() );
            StreamWriter writer = new StreamWriter( "times.xml" );
            serializer.Serialize( writer.BaseStream, Times );
            writer.Dispose();
        }

        public static times Load( this times Times ) {
            XmlSerializer serializer =
              new XmlSerializer( typeof( times ) );
            using ( Stream reader = new FileStream( "times.xml", FileMode.Open ) ) {
                Times = (times)serializer.Deserialize( reader );
            }
            return Times;
        }

    }





}
