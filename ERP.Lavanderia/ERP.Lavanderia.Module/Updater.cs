using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using ERP.Lavanderia.Module.PacoteEndereco;
using ERP.Lavanderia.Module.PacoteGeral;
using ERP.Lavanderia.Module.PacoteSeguranca;
using DevExpress.ExpressApp.Security;
using ERP.Lavanderia.Module.PacoteEmpresa;
using System.Collections.Generic;
using ERP.Lavanderia.Module.PacoteConfiguracoes;
using ERP.Lavanderia.Module.PacoteRoupa;
using ERP.Lavanderia.Module.PacoteLavagem;
using ERP.Lavanderia.Module.PacoteRecursosHumanos;
using ERP.Lavanderia.Module.PacoteMaterial;
using ERP.Lavanderia.Module.PacoteCaixa;
using DevExpress.ExpressApp.Reports;

namespace ERP.Lavanderia.Module
{
    public class Updater : ModuleUpdater
    {
        //private Dictionary<string, Action> atualizacoesAFazer;

        public Updater(ObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) {

            //atualizacoesAFazer = new Dictionary<string, Action>();
            //atualizacoesAFazer.Add("3.0.20", AtualizaHorario);

        }

        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

            #region Cria pais, estados e datas comemorativas padrao

            Pais pais = ObjectSpace.FindObject<Pais>(new BinaryOperator("NomePais", "Brasil"));
            if (pais == null)
            {
                pais = ObjectSpace.CreateObject<Pais>();

                pais.NomePais = "Brasil";
                pais.CodigoTelefone = "55";

                pais.Save();
            }

            #region Cria datas comemorativas padrao

            Feriado Jan0101 = Feriado.RetornaFeriadoPorDiaeMes(01, 01, ObjectSpace.Session);

            if (Jan0101 == null)
            {
                Jan0101 = ObjectSpace.CreateObject<Feriado>();
                Jan0101.Dia = 01;
                Jan0101.Mes = 01;
                Jan0101.Descricao = "Confraternização Universal";
                Jan0101.Save();

            }

            Feriado Abr2104 = Feriado.RetornaFeriadoPorDiaeMes(21, 04, ObjectSpace.Session);

            if (Abr2104 == null)
            {
                Abr2104 = ObjectSpace.CreateObject<Feriado>();
                Abr2104.Dia = 21;
                Abr2104.Mes = 04;
                Abr2104.Descricao = "Tiradentes";
                Abr2104.Save();

            }

            Feriado Mai0105 = Feriado.RetornaFeriadoPorDiaeMes(01, 05, ObjectSpace.Session);

            if (Mai0105 == null)
            {
                Mai0105 = ObjectSpace.CreateObject<Feriado>();
                Mai0105.Dia = 01;
                Mai0105.Mes = 05;
                Mai0105.Descricao = "Dia Mundial do Trabalho";
                Mai0105.Save();

            }

            Feriado Set0709 = Feriado.RetornaFeriadoPorDiaeMes(07, 09, ObjectSpace.Session);

            if (Set0709 == null)
            {
                Set0709 = ObjectSpace.CreateObject<Feriado>();
                Set0709.Dia = 07;
                Set0709.Mes = 09;
                Set0709.Descricao = "Independência do Brasil";
                Set0709.Save();

            }

            Feriado Out1210 = Feriado.RetornaFeriadoPorDiaeMes(12, 10, ObjectSpace.Session);

            if (Out1210 == null)
            {
                Out1210 = ObjectSpace.CreateObject<Feriado>();
                Out1210.Dia = 12;
                Out1210.Mes = 10;
                Out1210.Descricao = "Nossa Senhora Aparecida";
                Out1210.Save();

            }

            Feriado Nov0211 = Feriado.RetornaFeriadoPorDiaeMes(02, 11, ObjectSpace.Session);

            if (Nov0211 == null)
            {
                Nov0211 = ObjectSpace.CreateObject<Feriado>();
                Nov0211.Dia = 02;
                Nov0211.Mes = 11;
                Nov0211.Descricao = "Finados";
                Nov0211.Save();

            }

            Feriado Nov1511 = Feriado.RetornaFeriadoPorDiaeMes(15, 11, ObjectSpace.Session);

            if (Nov1511 == null)
            {
                Nov1511 = ObjectSpace.CreateObject<Feriado>();
                Nov1511.Dia = 15;
                Nov1511.Mes = 11;
                Nov1511.Descricao = "Proclamação da República";
                Nov1511.Save();

            }

            Feriado Dez2512 = Feriado.RetornaFeriadoPorDiaeMes(25, 12, ObjectSpace.Session);

            if (Dez2512 == null)
            {
                Dez2512 = ObjectSpace.CreateObject<Feriado>();
                Dez2512.Dia = 25;
                Dez2512.Mes = 12;
                Dez2512.Descricao = "Natal";
                Dez2512.Save();
            }



            #endregion

            #region Cria estados padrao

            XPCollection<Estado> listaEstados = pais.Estados;

            int totalEstados = listaEstados.Count;

            if (totalEstados != 27)
            {
                Estado ac = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Acre"));
                if (ac == null)
                {
                    ac = new Estado(ObjectSpace.Session);
                    ac.NomeEstado = "Acre";
                    ac.Sigla = "AC";
                    ac.Pais = pais;

                    ac.Save();
                }

                Estado al = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Alagoas"));
                if (al == null)
                {
                    al = new Estado(ObjectSpace.Session);
                    al.NomeEstado = "Alagoas";
                    al.Sigla = "AL";
                    al.Pais = pais;

                    al.Save();
                }

                Estado ap = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Amapá"));
                if (ap == null)
                {
                    ap = new Estado(ObjectSpace.Session);
                    ap.NomeEstado = "Amapá";
                    ap.Sigla = "AP";
                    ap.Pais = pais;

                    ap.Save();
                }

                Estado am = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Amazonas"));
                if (am == null)
                {
                    am = new Estado(ObjectSpace.Session);
                    am.NomeEstado = "Amazonas";
                    am.Sigla = "AM";
                    am.Pais = pais;

                    am.Save();
                }

                Estado ba = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Bahia"));
                if (ba == null)
                {
                    ba = new Estado(ObjectSpace.Session);
                    ba.NomeEstado = "Bahia";
                    ba.Sigla = "BA";
                    ba.Pais = pais;

                    ba.Save();
                }

                Estado ce = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Ceará"));
                if (ce == null)
                {
                    ce = new Estado(ObjectSpace.Session);
                    ce.NomeEstado = "Ceará";
                    ce.Sigla = "CE";
                    ce.Pais = pais;

                    ce.Save();
                }

                Estado df = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Distrito Federal"));
                if (df == null)
                {
                    df = new Estado(ObjectSpace.Session);
                    df.NomeEstado = "Distrito Federal";
                    df.Sigla = "DF";
                    df.Pais = pais;

                    df.Save();
                }

                Estado es = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Espírito Santo"));
                if (es == null)
                {
                    es = new Estado(ObjectSpace.Session);
                    es.NomeEstado = "Espírito Santo";
                    es.Sigla = "ES";
                    es.Pais = pais;

                    es.Save();
                }

                Estado go = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Goiás"));
                if (go == null)
                {
                    go = new Estado(ObjectSpace.Session);
                    go.NomeEstado = "Goiás";
                    go.Sigla = "GO";
                    go.Pais = pais;

                    go.Save();
                }

                Estado ma = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Maranhão"));
                if (ma == null)
                {
                    ma = new Estado(ObjectSpace.Session);
                    ma.NomeEstado = "Maranhão";
                    ma.Sigla = "MA";
                    ma.Pais = pais;

                    ma.Save();
                }

                Estado mt = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Mato Grosso"));
                if (mt == null)
                {
                    mt = new Estado(ObjectSpace.Session);
                    mt.NomeEstado = "Mato Grosso";
                    mt.Sigla = "MT";
                    mt.Pais = pais;

                    mt.Save();
                }

                Estado ms = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Mato Grosso do Sul"));
                if (ms == null)
                {
                    ms = new Estado(ObjectSpace.Session);
                    ms.NomeEstado = "Mato Grosso do Sul";
                    ms.Sigla = "MS";
                    ms.Pais = pais;

                    ms.Save();
                }

                Estado mg = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Minas Gerais"));
                if (mg == null)
                {
                    mg = new Estado(ObjectSpace.Session);
                    mg.NomeEstado = "Minas Gerais";
                    mg.Sigla = "MG";
                    mg.Pais = pais;

                    mg.Save();
                }

                Estado pa = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Pará"));
                if (pa == null)
                {
                    pa = new Estado(ObjectSpace.Session);
                    pa.NomeEstado = "Pará";
                    pa.Sigla = "PA";
                    pa.Pais = pais;

                    pa.Save();
                }

                Estado pb = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Paraíba"));
                if (pb == null)
                {
                    pb = new Estado(ObjectSpace.Session);
                    pb.NomeEstado = "Paraíba";
                    pb.Sigla = "PB";
                    pb.Pais = pais;

                    pb.Save();
                }

                Estado pr = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Paraná"));
                if (pr == null)
                {
                    pr = new Estado(ObjectSpace.Session);
                    pr.NomeEstado = "Paraná";
                    pr.Sigla = "PR";
                    pr.Pais = pais;

                    pr.Save();
                }

                Estado pe = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Pernambuco"));
                if (pe == null)
                {
                    pe = new Estado(ObjectSpace.Session);
                    pe.NomeEstado = "Pernambuco";
                    pe.Sigla = "PE";
                    pe.Pais = pais;

                    pe.Save();
                }

                Estado pi = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Piauí"));
                if (pi == null)
                {
                    pi = new Estado(ObjectSpace.Session);
                    pi.NomeEstado = "Piauí";
                    pi.Sigla = "PI";
                    pi.Pais = pais;

                    pi.Save();
                }

                Estado rj = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Rio de Janeiro"));
                if (rj == null)
                {
                    rj = new Estado(ObjectSpace.Session);
                    rj.NomeEstado = "Rio de Janeiro";
                    rj.Sigla = "PI";
                    rj.Pais = pais;

                    rj.Save();
                }

                Estado rn = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Rio Grande do Norte"));
                if (rn == null)
                {
                    rn = new Estado(ObjectSpace.Session);
                    rn.NomeEstado = "Rio Grande do Norte";
                    rn.Sigla = "RN";
                    rn.Pais = pais;

                    rn.Save();
                }

                Estado rs = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Rio Grande do Sul"));
                if (rs == null)
                {
                    rs = new Estado(ObjectSpace.Session);
                    rs.NomeEstado = "Rio Grande do Sul";
                    rs.Sigla = "RN";
                    rs.Pais = pais;

                    rs.Save();
                }

                Estado ro = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Rondônia"));
                if (ro == null)
                {
                    ro = new Estado(ObjectSpace.Session);
                    ro.NomeEstado = "Rondônia";
                    ro.Sigla = "RO";
                    ro.Pais = pais;

                    ro.Save();
                }

                Estado rr = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Roraima"));
                if (rr == null)
                {
                    rr = new Estado(ObjectSpace.Session);
                    rr.NomeEstado = "Roraima";
                    rr.Sigla = "RR";
                    rr.Pais = pais;

                    rr.Save();
                }

                Estado sc = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Santa Catarina"));
                if (sc == null)
                {
                    sc = new Estado(ObjectSpace.Session);
                    sc.NomeEstado = "Santa Catarina";
                    sc.Sigla = "SC";
                    sc.Pais = pais;

                    sc.Save();
                }

                Estado sp = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "São Paulo"));
                if (sp == null)
                {
                    sp = new Estado(ObjectSpace.Session);
                    sp.NomeEstado = "São Paulo";
                    sp.Sigla = "SP";
                    sp.Pais = pais;

                    sp.Save();
                }

                Estado se = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Sergipe"));
                if (se == null)
                {
                    se = new Estado(ObjectSpace.Session);
                    se.NomeEstado = "Sergipe";
                    se.Sigla = "SE";
                    se.Pais = pais;

                    se.Save();
                }

                Estado to = ObjectSpace.FindObject<Estado>(new BinaryOperator("NomeEstado", "Tocantins"));
                if (to == null)
                {
                    to = new Estado(ObjectSpace.Session);
                    to.NomeEstado = "Tocantins";
                    to.Sigla = "TO";
                    to.Pais = pais;

                    to.Save();
                }

            }

            #endregion


            #endregion

            #region Cria papéis e usuario padrao
            Papel.CriarPapeis(ObjectSpace.Session);
            
            // If a user named 'Sam' doesn't exist in the database, create this user
            Usuario user1 = ObjectSpace.FindObject<Usuario>(new BinaryOperator("UserName", "Administrador"));
            if (user1 == null)
            {
                user1 = new Usuario(ObjectSpace.Session);
                user1.UserName = "administrador";
                user1.ChangePasswordOnFirstLogon = false;
                //user1.FirstName = "Administrador";
                // Set a password if the standard authentication type is used
                user1.SetPassword("senha");
            }

            Papel adminRole = Papel.RetornaPapel(TipoPapelLavanderia.Administrador, ObjectSpace.Session);
            
            // Add the Administrators role to the user1
            user1.Roles.Add(adminRole);

            // Save the users to the database
            user1.Save();

            #endregion

            #region Configurações

            XPCollection<ConfiguracaoGeral> listGeral = new XPCollection<ConfiguracaoGeral>(ObjectSpace.Session);
            if (listGeral.Count == 0)
            {
                ConfiguracaoGeral cfgGeral = new ConfiguracaoGeral(ObjectSpace.Session);
                cfgGeral.SenhaSmtp = "";
                cfgGeral.ServidorSmtp = "";
                cfgGeral.UsuarioSmtp = "";
                cfgGeral.UtilizarAutenticacaoSmtp = false;

                cfgGeral.DiaMensagemAniversario = 0;
                cfgGeral.MensagemAniversario = "";

                cfgGeral.DiasParaEntrega = 2;

                cfgGeral.Save();
            }

            #endregion

            #region Cria empresa Padrão

            Empresa empresa = Empresa.RetornaEmpresa(ObjectSpace.Session);

            if (empresa == null) {
                empresa = new Empresa(ObjectSpace.Session);
                
                empresa.Pessoa = new PacotePessoa.Pessoa(ObjectSpace.Session);
                empresa.Pessoa.TipoPessoa = PacotePessoa.TipoPessoa.Juridica;
                empresa.Pessoa.Nome = "Empresa Padrão";

                empresa.Save();
            }

            #endregion

            #region Cria Departamentos Padrão
            
            string[] departamentosPadrao = new string[] {"Recepção", "Gerência", "Operação", "Presidência"};

            foreach (string t in departamentosPadrao)
            {
                Departamento dep = Departamento.RetornaDepartamentoPorNomeEEmpresa(t, empresa, ObjectSpace.Session);
                if (dep == null)
                {
                    dep = new Departamento(ObjectSpace.Session);
                    dep.Nome = t;
                    dep.Empresa = empresa;
                    dep.Save();
                }
            }

            #endregion

            #region Cria caixa padrão

            var caixas = Caixa.RetornaCaixas(Session);

            if (caixas == null || caixas.Count == 0) {
                Caixa caixa = new Caixa(Session);
                caixa.Nome = "Caixa Padrão";
                caixa.Save();
            }

            #endregion

            #region Cria ponto de coleta padrão

            var pontosDeColeta = PontoDeColeta.RetornaPontosDeColeta(Session);

            if (pontosDeColeta == null || pontosDeColeta.Count == 0)
            {
                PontoDeColeta caixa = new PontoDeColeta(Session);
                caixa.Nome = "Ponto de Coleta Padrão";
                caixa.Save();
            }

            #endregion

            #region Criar Tipos de pacotes de roupa padrão

            String tipoDePacote1 = "Pacote de roupas dobradas";
            String tipoDePacote2 = "Pacote de Cabides";

            if (TipoPacoteDeRoupa.RetornaTipoDePacoteDeRoupa(Session, tipoDePacote1) == null)
            {
                TipoPacoteDeRoupa tipo = new TipoPacoteDeRoupa(Session);
                tipo.Descricao = tipoDePacote1;
                tipo.Save();
            }

            if (TipoPacoteDeRoupa.RetornaTipoDePacoteDeRoupa(Session, tipoDePacote2) == null)
            {
                TipoPacoteDeRoupa tipo = new TipoPacoteDeRoupa(Session);
                tipo.Descricao = tipoDePacote2;
                tipo.Save();
            }

            #endregion

            #region Cria tipos de roupa padrão

            string[] tiposDeRoupa = new string[] {"Calça", "Vestido", "Bermuda", "Camisa", "Paletó", "Blazer", "Lençol", "Colcha", 
                "Toalha de Rosto", "Toalha de Banho", "Meia", "Cueca", "Calcinha"};

            foreach (string t in tiposDeRoupa) {
                if (Tipo.RetornaTipo(Session, t) == null)
                {
                    Tipo tipo = new Tipo(Session);
                    tipo.Descricao = t;
                    tipo.Save();
                }
            }

            #endregion

            #region Cria cores de roupa padrão

            string[] coresDeRoupa = new string[] {"Azul", "Amarelo", "Vermelho", "Verde", "Branco", "Preto", "Cinza", "Laranja"};

            foreach (string s in coresDeRoupa)
            {
                if (Cor.RetornaCor(Session, s) == null)
                {
                    var obj = new Cor(Session);
                    obj.Descricao = s;
                    obj.Save();
                }
            }

            #endregion

            #region Cria tamanho de roupa padrão

            string[] tamanhosDeRoupa = new string[] { "P", "M", "G", "GG", "XG", "Baby Look", "40", "42", "44", "48", "50", 
                "1", "2", "3", "4", "5", "Casal", "Solteiro", "King" };

            foreach (string s in tamanhosDeRoupa)
            {
                if (Tamanho.RetornaTamanho(Session, s) == null)
                {
                    var obj = new Tamanho(Session);
                    obj.Descricao = s;
                    obj.Save();
                }
            }

            #endregion

            #region Cria cargos padrão

            string[] cargosDeColaborador = new string[] { "Lavadeira", "Passadeira", "Atendente", "Gerente de Operações", "Gerente Geral", "Entregador" };

            foreach (string s in cargosDeColaborador)
            {
                if (ColaboradorCargo.RetornaCargo(Session, s) == null)
                {
                    var obj = new ColaboradorCargo(Session);
                    obj.Descricao = s;
                    obj.Save();
                }
            }

            #endregion

            #region Cria material padrão

            string[] material = new string[] { "Sabão em Pó", "Sabão em Pedra", "Amaciante", "Cheirinho", "Cabide", "Cloro", "Álcool" };

            foreach (string s in material)
            {
                if (Material.RetornaMaterial(Session, s) == null)
                {
                    var obj = new Material(Session);
                    obj.Nome = s;
                    obj.Save();
                }
            }

            #endregion

            #region Cria capital padrão

            string[] capitalPadrao = new string[] { "Dinheiro", "Cartão de Crédito", "Cartão de Débito", "Cheque" };

            foreach (string s in capitalPadrao)
            {
                if (Capital.RetornaCapital(Session, s) == null)
                {
                    var obj = new Capital(Session);
                    obj.Descricao = s;
                    obj.Save();
                }
            }

            #endregion

            #region Cria tipo de movimentação padrão

            string[] tipoDeMovimentacaoPadrao = new string[] { "Pagamento de Lavagem", "Salário de Funcionário", "Pagamento de Almoço", "Compra de Material", 
                "Pagamento de Conta Energia", "Pagamento de Conta Água" };

            foreach (string s in tipoDeMovimentacaoPadrao)
            {
                if (TipoMovimentacao.RetornaTipoMovimentacao(Session, s) == null)
                {
                    var obj = new TipoMovimentacao(Session);
                    obj.Descricao = s;
                    obj.Save();
                }
            }

            #endregion

            #region Cria tecidos padrão

            string[] tecidosDeRoupaPadrao = new string[] { "Linho", "Jeans", "Malha", "Seda" };

            foreach (string s in tecidosDeRoupaPadrao)
            {
                if (Tecido.RetornaTecido(Session, s) == null)
                {
                    var obj = new Tecido(Session);
                    obj.Descricao = s;
                    obj.Save();
                }
            }

            #endregion

            #region Cria Relatórios

            CreateReport("Lavagem", "Lavagem");

            #endregion

            AtualizaBanco();
        }

        private void CreateReport(string reportFile, string reportName)
        {
            ReportData reportdata = ObjectSpace.FindObject<ReportData>(new BinaryOperator("Name", reportName));

            if (reportdata != null)
            {
                reportdata.Delete();
                reportdata.Session.CommitTransaction();
            }

            reportdata = ObjectSpace.CreateObject<ReportData>();
            XafReport rep = new XafReport();
            rep.ObjectSpace = ObjectSpace;

            rep.LoadLayout(GetType().Assembly.GetManifestResourceStream("ERP.Lavanderia.Module.PacoteRelatorios." + reportFile + ".repx"));
            rep.ReportName = reportName;
            reportdata.SaveXtraReport(rep);
            reportdata.Save();
        }

         private void AtualizaBanco()
        {
            //foreach (var atualizacao in atualizacoesAFazer)
            //{
            //    //atualizacao.Value();
            //}
        }
    }
}
