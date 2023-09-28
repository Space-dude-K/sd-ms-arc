using Api_pdc_Entities.PrintDevice;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Api_pdc_Entities.Enums;

namespace Api_pdc_Parser
{
    public class Parser : IParser
    {
        private readonly ILogger<Parser> _logger;

        public Parser(ILogger<Parser> logger)
        {
            _logger = logger;
        }

        public Task<Printer> ParseRawDataFromFile(string filePath, PrinterType printerType)
        {
            Printer ppd = new Printer();

            // TODO - C# 9 or istead of case stacking (maybe?).
            switch (printerType)
            {
                case PrinterType.DefaultType:
                    break;
                case PrinterType.LexmarkMX421ade:
                case PrinterType.LexmarkMS421dn:
                case PrinterType.LexmarkMB2236adw:
                    ppd = ParseLexmarkPrinterData(filePath);
                    break;
                case PrinterType.XeroxWorkCentre3325:
                    break;
                case PrinterType.XeroxPhaser4620:
                    break;
                case PrinterType.HpLaserJet4250:
                    break;
                case PrinterType.HpLaserJetP3005:
                    break;
                case PrinterType.KyoceraEcosysP5021cdn:
                    break;
                case PrinterType.CanonLBP6670:
                    break;
                case PrinterType.OkiMB491:
                    ppd = ParseOki491PrinterData(filePath);
                    break;
                default:
                    break;
            }

            return Task.FromResult(ppd);
        }
        public Printer ParseLexmarkPrinterData(string filePath)
        {
            HtmlDocument doc = null;
            Printer ppd = null;

            try
            {
                doc = new HtmlWeb().Load(filePath);

                var rawNumberOfPages =
                        doc.DocumentNode
                               .SelectSingleNode("//table[tr/td/b/font[.='Media&nbsp;Printed&nbsp;Side&nbsp;Count']]/following-sibling::table[12]/tr/td[2]")
                               .InnerText;
                var rawTonerStatus =
                        doc.DocumentNode
                               .SelectSingleNode("//table[tr/td/b/font[.='Black&nbsp;Cartridge']]/following-sibling::table[4]/tr/td[2]")
                               .InnerText.Replace("%", "");
                var rawDrumStatus =
                        doc.DocumentNode
                               .SelectSingleNode("//table[tr/td/b/font[.='Imaging&nbsp;Unit']]/following-sibling::table[2]/tr/td[2]")
                               .InnerText.Replace("%", "");

                ppd = new Printer();

                int.TryParse(rawNumberOfPages, out int parsedNumberOfPages);
                ppd.NumberOfPages = parsedNumberOfPages;

                int.TryParse(rawTonerStatus, out int parsedTonerStatus);
                ppd.TonerLevel = parsedTonerStatus;

                int.TryParse(rawDrumStatus, out int parsedDrumStatus);
                ppd.DrumLevel = parsedDrumStatus;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (doc != null)
                    doc = null;
            }

            //Console.WriteLine(ppd.NumberOfPages + " " + ppd.TonerLevel + " " + ppd.DrumLevel);
            return ppd;
        }
        public Printer ParseOki491PrinterData(string filePath)
        {
            HtmlDocument doc = null;
            Printer ppd = null;

            try
            {
                doc = new HtmlWeb().Load(filePath);

                var rawNumberOfPages =
                    doc.DocumentNode
                           .SelectSingleNode("//table[tr/td/nobr[.='Total Impressions:']]/tr[10]/td[2]").InnerText;
                var rawTonerStatus =
                        doc.DocumentNode
                               .SelectSingleNode("//input[@name='AVAILABELBLACKTONER']").Attributes["value"].Value;
                var rawDrumStatus =
                        doc.DocumentNode
                               .SelectSingleNode("//tr[td[.='Drum Unit :']]/td[2]").InnerText.Replace("%", "");

                ppd = new Printer();

                int.TryParse(rawNumberOfPages, out int parsedNumberOfPages);
                ppd.NumberOfPages = parsedNumberOfPages;

                int.TryParse(rawTonerStatus, out int parsedTonerStatus);
                ppd.TonerLevel = parsedTonerStatus;

                int.TryParse(rawDrumStatus, out int parsedDrumStatus);
                ppd.DrumLevel = parsedDrumStatus;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (doc != null)
                    doc = null;
            }

            return ppd;
        }
    }
}