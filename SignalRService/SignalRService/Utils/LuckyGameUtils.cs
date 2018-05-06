﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public class LuckyGameUtils
    {
        public static List<LuckyGameCard>GetCards()
        {
            List<LuckyGameCard> cards = new List<LuckyGameCard>();
            cards.Add(new LuckyGameCard() { Id = 1, Key = "lkgc1", Symbol = "fas fa-anchor fa-10x" });
            cards.Add(new LuckyGameCard() { Id = 2, Key = "lkgc2", Symbol = "fab fa-jedi-order fa-10x" });
            cards.Add(new LuckyGameCard() { Id = 3, Key = "lkgc3", Symbol = "fab fa-affiliatetheme fa-10x" });
            cards.Add(new LuckyGameCard() { Id = 4, Key = "lkgc4", Symbol = "far fa-eye fa-10x" });
            cards.Add(new LuckyGameCard() { Id = 5, Key = "lkgc5", Symbol = "fab fa-angular fa-10x" });
            cards.Add(new LuckyGameCard() { Id = 6, Key = "lkgc6", Symbol = "fab fa-500px fa-10x" });
            cards.Add(new LuckyGameCard() { Id = 7, Key = "lkgc7", Symbol = "fab fa-angellist fa-10x" });
            return cards;
        }

        private static Random random = new Random();
        public static LuckyGameCard GetRandomCard()
        {
            var cards = GetCards();
            int randint = random.Next(0, cards.Count -1);
            return cards[randint];
        }
    }

    public class LuckyGameCard
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Symbol { get; set; }
    }

    public class LuckyGameCardResult
    {
        public List<LuckyGameCard> Cards { get; set; }
        public float WinFactor { get; set; }
        public double AmountWon { get; set; }
        public double AmountLost { get; set; }
        public double UserTotalAmount { get; set; }
        public double TotalAmountAvailable { get; set; }
        public int ErrorNumber { get; set; }
        public string Message { get; set; }
    }
}