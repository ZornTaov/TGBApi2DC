using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Diagnostics;
using System.Security.Policy;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TGBApi;

namespace TGBApi2StructC
{
    class TGBApi2StructC
    {
        static List<TypeFields> getTypeGroup(string typeName, List<Types> typesList, out List<Types> typesListOut)
        {
            typesListOut = (from type in typesList
                            where type.name.Contains(typeName)
                            && !type.name.Equals(typeName)
                            select type).ToList();
            List<List<TypeFields>> fields = (from type in typesListOut select type.fields).ToList();
            return fields.Aggregate((x, y) => x.Where(xi => y.Select(yi => yi.name).Contains(xi.name)).ToList());
        }

        public static void GenerateContracts(rootJson result)
        {
            /// InputMedia setup
            List<Types> inputMediasTypes;
            List<TypeFields> inputMediaCommonFields = getTypeGroup("InputMedia", result.types, out inputMediasTypes);

            /// InlineQueryResult setup
            List<Types> inlineQueryResultTypes;
            List<TypeFields> inlineQueryResultCommonFields = getTypeGroup("InlineQueryResult", result.types, out inlineQueryResultTypes);

            /// PassportElementError setup
            List<Types> PassportElementErrorTypes;
            List<TypeFields> PassportElementErrorCommonFields = getTypeGroup("PassportElementError", result.types, out PassportElementErrorTypes);

            /// Keyboard setup
            List<Types> keyboardsTypes;
            getTypeGroup("KeyboardMarkup", result.types, out keyboardsTypes);

            foreach (var item in keyboardsTypes)
            {
                item.fields[0].altProperty = "Keyboard";
                item.fields[0].type = new string[] { "List<List<InlineKeyboardButton>>" };
            }
            result.types.Add(new Types()
            {
                name = "KeyboardMarkup",
                description = "A basic Keyboard object",
                fields = new List<TypeFields>() {
                    new TypeFields() {
                        name = "Keyboard",
                        description = "The keyboard object that the child keyboard classes will reference, makes extention classes easier",
                        type = new string[] { "List<List<InlineKeyboardButton>>" },
                        defaultValue = " = new List<List<InlineKeyboardButton>>(100);",
                        hasHeader = false
                    }
                }
            });

            /// Sorting into folders
            Dictionary<string, string> folderNames = new Dictionary<string, string>()
            {
                { "Update", "Update" },
                { "Webhook", "Update" },
                { "Chat", "Chat" },
                { "Keyboard", "Keyboard" },
                { "InputMedia", "InputMedia" },
                { "Inline", "Inline" },
                { "MessageContent", "Inline" },
                { "Sticker", "Sticker" },
                { "MaskPosition", "Sticker" },
                { "LabeledPrice", "Payments" },
                { "Invoice", "Payments" },
                { "Shipping", "Payments" },
                { "OrderInfo", "Payments" },
                { "SuccessfulPayment", "Payments" },
                { "PreCheckoutQuery", "Payments" },
                { "Passport", "Passport" },
                { "EncryptedCredentials", "Passport" },
                { "Game", "Game" },
                { "Dice", "Game" },
                { "Poll", "Poll" }
            };
            /// edge cases: int id should be long id
            List<string> idNames = new List<string>
            {
                "update_id",
                "id",
                "message_id",
                "forward_from_message_id",
                "migrate_to_chat_id",
                "migrate_from_chat_id",
                "user_id",
                "file_date",
                "file_size",
                "until_date",
                "close_date",
                "edit_date",
                "forward_date",
                "date",
                "last_error_date"
            };

            Directory.CreateDirectory(@".\StructC");
            Directory.SetCurrentDirectory(@".\StructC");

            Directory.CreateDirectory(@".\Contracts");
            foreach (Types type in result.types)
            {
                string fieldPath = "Contracts";
                foreach (var item in folderNames)
                {
                    if (type.name.Contains(item.Key))
                    {
                        fieldPath += @"\" + item.Value;
                        Directory.CreateDirectory(@".\" + fieldPath);
                        break;
                    }
                }
                using (StreamWriter sw = File.CreateText(String.Format(@".\{0}\{1}.h", fieldPath, type.name)))
                {
                    sw.WriteLine(StructCStringFormats.LICENSE, DateTime.Now.Year);
                    /*if (type.name.Equals("InputFile"))
                    {
                        sw.WriteLine(StructCStringFormats.HTTP_HEADER);
                    }
                    if (type.name.Contains("KeyboardMarkup"))
                    {
                        sw.WriteLine(StructCStringFormats.LIST_HEADER);
                    }*/
                    //sw.WriteLine(StructCStringFormats.FILE_HEADER);
                    //sw.WriteLine("{");
                    if (type.name.Contains("LoginUrl"))
                    {
                        type.description = Regex.Replace(type.description, @"\r\n?|\n", " ");
                    }
                    sw.WriteLine(StructCStringFormats.SUMMARY, type.description, "");
                    /// edge cases:
                    {
                        /// InputFile
                        if (type.name == "InputFile")
                        {
                            //sw.WriteLine(StructCStringFormats.StructC_HEADER_INHERITS, type.name, "StreamContent");
                        }
                        /// InputMedia
                        else if (type.name.StartsWith("InputMedia"))
                        {
                            switch (type.name)
                            {
                                /// commons are type, media, caption, parse_mode, sometimes thumb
                                case "InputMedia":
                                    foreach (var item in inputMediasTypes)
                                    {
                                        //sw.WriteLine(StructCStringFormats.KNOWN_TYPE, item.name);
                                    }
                                    sw.WriteLine(StructCStringFormats.StructC_HEADER, type.name);
                                    type.fields = inputMediaCommonFields.Select(x => { x.description = x.description.Replace("photo", "media"); return x; }).ToList();
                                    if(type.fields.Count > 0)type.fields[0].description = "Type of the result";
                                    break;
                                default:
                                    sw.WriteLine(StructCStringFormats.StructC_HEADER_INHERITS, type.name, "InputMedia");
                                    type.fields = type.fields.Where(x => !inputMediaCommonFields.Any(y => x.name.Equals(y.name))).ToList();
                                    break;
                            }
                        }
                        /// InlineQueryResult
                        else if (type.name.StartsWith("InlineQueryResult"))
                        {
                            switch (type.name)
                            {
                                /// commons are id, type
                                case "InlineQueryResult":
                                    foreach (var item in inlineQueryResultTypes)
                                    {
                                        //sw.WriteLine(StructCStringFormats.KNOWN_TYPE, item.name);
                                    }
                                    sw.WriteLine(StructCStringFormats.StructC_HEADER, type.name);
                                    type.fields = inlineQueryResultCommonFields;

                                    break;
                                default:
                                    sw.WriteLine(StructCStringFormats.StructC_HEADER_INHERITS, type.name, "InlineQueryResult");
                                    type.fields = type.fields.Where(x => !inlineQueryResultCommonFields.Any(y => x.name.Equals(y.name))).ToList();
                                    break;
                            }
                        }
                        /// PassportElementError
                        else if (type.name.StartsWith("PassportElementError"))
                        {
                            switch (type.name)
                            {
                                /// commons are id, type
                                case "PassportElementError":
                                    foreach (var item in PassportElementErrorTypes)
                                    {
                                        //sw.WriteLine(StructCStringFormats.KNOWN_TYPE, item.name);
                                    }
                                    sw.WriteLine(StructCStringFormats.StructC_HEADER, type.name);
                                    type.fields = PassportElementErrorCommonFields;
                                    break;
                                default:
                                    sw.WriteLine(StructCStringFormats.StructC_HEADER_INHERITS, type.name, "PassportElementError");
                                    type.fields = type.fields.Where(x => !PassportElementErrorCommonFields.Any(y => x.name.Equals(y.name))).ToList();
                                    break;
                            }
                        }
                        /// InlineKeyboardMarkup
                        /// ReplyKeyboardMarkup
                        else if (type.name.Contains("KeyboardMarkup"))
                        {
                            switch (type.name)
                            {
                                /// commons are id, type
                                case "KeyboardMarkup":
                                    foreach (var item in keyboardsTypes)
                                    {
                                        //sw.WriteLine(StructCStringFormats.KNOWN_TYPE, item.name);
                                    }
                                    sw.WriteLine(StructCStringFormats.StructC_HEADER, type.name);
                                    break;
                                default:
                                    sw.WriteLine(StructCStringFormats.StructC_HEADER_INHERITS, type.name, "KeyboardMarkup");
                                    break;
                            }
                        }
                        else
                        {
                            sw.WriteLine(StructCStringFormats.StructC_HEADER, type.name);
                        }
                        sw.WriteLine("{");
                        if (type.name.Equals("InputFile"))
                        {
                            //sw.WriteLine(StructCStringFormats.SUMMARY, "Input File Stream", "\t");
                            //sw.WriteLine(StructCStringFormats.CONSTRUCTOR, type.name, "Stream", "stream");
                        }

                        foreach (TypeFields field in type.fields)
                        {
                            // edge case fields:
                            // InputMedia.caption = ""
                            // InputMedia.parse_mode = ""
                            // PhotoSize.file_size type = long
                            // Message.*date* type = long
                            // Message.edit_date = 0
                            // Message.migrate_to_chat_id = 0
                            // Message.migrate_from_chat_id = 0
                            if (type.name.Equals("InputMedia"))
                            {
                                if (field.name.Equals("caption"))
                                {
                                    field.defaultValue = " = \"\";";
                                }
                                if (field.name.Equals("parse_mode"))
                                {
                                    field.defaultValue = " = \"\";";
                                }
                            }
                            string fieldType = field.type.Length > 1 ? "object" : field.type[0];

                            if (type.name.Equals("PhotoSize"))
                            {
                                if (field.name.Contains("file_size"))
                                {
                                    fieldType = "long";
                                }
                            }
                            if (type.name.Equals("Message"))
                            {
                                if (field.name.Contains("date"))
                                {
                                    fieldType = "int64_t";
                                }
                                if (field.name.Equals("edit_date"))
                                {
                                    field.defaultValue = " = 0;";
                                }
                                if (field.name.Equals("migrate_to_chat_id"))
                                {
                                    field.defaultValue = " = 0;";
                                }
                                if (field.name.Equals("migrate_from_chat_id"))
                                {
                                    field.defaultValue = " = 0;";
                                }
                            }
                            if (idNames.Contains(field.name) && fieldType == "int")
                            {
                                fieldType = "int64_t";
                            }
                            if (fieldType == "True")
                            {
                                fieldType = "bool";
                            }
                            sw.WriteLine(StructCStringFormats.SUMMARY, field.description.Replace(@"\n", " "), "\t");
                            /*if (field.hasHeader)
                            {
                                sw.WriteLine(StructCStringFormats.DM_FIELD_HEADER,
                                    field.name,
                                    field.description.StartsWith("Optional.") ? StructCStringFormats.DM_EMIT_DEFAULT : StructCStringFormats.DM_REQUIRED
                                );
                            }*/
                            string[] baseTypes = { "int", "bool", "string", "char", "byte", "int64_t" };
                            
                            if (fieldType.Contains("Array"))
                            {
                                fieldType = fieldType.Replace("Array<", "").Replace(">", "[]");

                            }
                            if (fieldType.Contains("List"))
                            {
                                fieldType = fieldType.Replace("List<", "").Replace(">", "[]");

                            }

                            if (baseTypes.Contains(fieldType))
                                sw.WriteLine( StructCStringFormats.DM_FIELD,
                                    field.name,
                                    fieldType.Contains("string") ? "char *" : fieldType,
                                    field.defaultValue != null ? field.defaultValue : ""
                                );
                            else
                            {
                                sw.WriteLine(StructCStringFormats.DM_FIELD,
                                    field.name,
                                    fieldType.Contains("[]")? fieldType + "*": fieldType + "_t*",
                                    field.defaultValue != null ? field.defaultValue : "= NULL"
                                );
                            }
                        }
                        sw.WriteLine("}} {0}_t;", type.name);
                    }
                    //sw.WriteLine("}");
                }
            }


            Directory.SetCurrentDirectory(@"..");
        }
    }
}
