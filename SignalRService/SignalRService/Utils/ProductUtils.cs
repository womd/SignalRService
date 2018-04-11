﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web;

namespace SignalRService.Utils
{
    public static class ProductUtils
    {
        public static int calc_percent(int count, int total)
        {
            var perc = count * 100 / total;
            return perc;
        }

        public static bool IsValidProductData(Hubs.ProductData data, out List<string>messages)
        {

            messages = new List<string>();
            int idanger = 0;

            if (Utils.ValidationUtils.IsDangerousString(data.Id, out idanger))
            {
                messages.Add("Invalid Id given...");
                return false;
            }
            if (string.IsNullOrEmpty(data.Name))
            {
                messages.Add("Name darf nicht leer sein.");
                return false;
            }

            if (data.Name.Length < 2 && data.Name.Length > 120)
            {
                messages.Add("Name soll 2 - 120 Zeichen haben.");
                return false;
            }

            if (Utils.ValidationUtils.IsDangerousString(data.Name, out idanger))
            {
                messages.Add("Name darf keine gefährlichen Zeichen enthalten.");
                return false;
            }

            if (string.IsNullOrEmpty(data.Description))
            {
                messages.Add("Beschreibung darf nicht leer sein.");
                return false;
            }

            if (data.Description.Length < 2 && data.Description.Length > 200)
            {
                messages.Add("Name soll 2 - 200 Zeichen haben.");
                return false;
            }

            if (Utils.ValidationUtils.IsDangerousString(data.Description, out idanger))
            {
                messages.Add("Name darf keine gefährlichen Zeichen enthalten.");
                return false;
            }

            if (data.Price > 100000 || data.Price < 0)
            {
                messages.Add("Preis soll zwischen 0 und 100000 sein.");
                return false;
            }

            if (data.PartNumber != string.Empty)
            {
                if (Utils.ValidationUtils.IsDangerousString(data.PartNumber, out idanger))
                {
                    messages.Add("Teilenummer darf keine gefährlichen Zeichen enthalten.");
                    return false;
                }
            }

            return true;
        }
    }
}