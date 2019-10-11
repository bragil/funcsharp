﻿using NUnit.Framework;
using Shouldly;
using System;
using System.Diagnostics;

namespace FunSharp.Tests
{
    [TestFixture]
    public class ThenTests
    {
        [Test]
        public void Deve_Ter_Novo_Valor_Apos_Then()
        {
            var res1 = Res.Of(256);

            var newRes = res1.Then(n => n.ToString());

            var valor = newRes.Match(
                                    s => s,
                                    e => e.Message,
                                    _ => ""
                                );

            newRes.IsSome.ShouldBeTrue();
            newRes.IsError.ShouldBeFalse();
            newRes.IsNone.ShouldBeFalse();

            valor.ShouldBe("256");
        }


        [Test]
        public void Deve_Ser_Unit_Apos_Then()
        {
            var res = Res.Of(500)
                         .Then(n => Debug.WriteLine("text"));

            res.IsSome.ShouldBeTrue();
            res.IsError.ShouldBeFalse();
            res.IsNone.ShouldBeFalse();

            var value = res.Match(
                            some: v => v.GetType().Name,
                            error: e => e.GetType().Name,
                            none: _ => "None"
                        );

            value.ShouldBe("Unit");
        }

        [Test]
        public void Deve_Ser_String_Apos_Unit()
        {
            var res = Res.Of(500)
                         .Then(n => Debug.WriteLine("text"))
                         .Then(u => u.GetType().Name);

            res.IsSome.ShouldBeTrue();
            res.IsError.ShouldBeFalse();
            res.IsNone.ShouldBeFalse();

            var value = res.GetValueOrElse("");
            value.ShouldBe("Unit");
        }

        [Test]
        public void Deve_Ter_Valor_Do_Ultimo_Then()
        {
            var res = Res.Of(new Teste())
                         .Then(t => t.Soma(10, 20))
                         .Then(n => n * 300D)
                         .Then(d => Convert.ToDecimal(d))
                         .Then(m => m.ToString())
                         .Then(s => s.ToCharArray())
                         .Then(arr => arr.Length);

            Assert.AreEqual(4, res.GetValueOrElse(-1));
        }

    }
}
