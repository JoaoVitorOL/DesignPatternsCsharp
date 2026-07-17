// Bridge Coding Exercise
// You are given an example of an inheritance hierarchy 
// which results in Cartesian-product duplication.

// Please refactor this hierarchy, 
// giving the base class Shape  a constructor 
// that takes an interface IRenderer  defined as

// interface IRenderer
// {
//   string WhatToRenderAs { get; }
// }
// as well as VectorRenderer  and RasterRenderer  classes. 
// Each implementer of the Shape  abstract class 
// should have a constructor that takes an IRenderer  such that,
// subsequently, each constructed object's ToString()  operates correctly, 
// for example,
// 
// new Triangle(new RasterRenderer()).ToString() // returns "Drawing Triangle as pixels" 



using System;

namespace Coding.Exercise
{

// 1. Definição da interface de renderização conforme o enunciado
    public interface IRenderer
    {
        string WhatToRenderAs { get; }
    }

    // 2. Implementadores concretos da "Ponte" (Bridge)
    public class VectorRenderer : IRenderer
    {
        public string WhatToRenderAs => "lines";
    }

    public class RasterRenderer : IRenderer
    {
        public string WhatToRenderAs => "pixels";
    }

    // 3. Classe base Abstrata refatorada para aceitar a ponte no construtor
    public abstract class Shape
    {
        protected IRenderer renderer;
        public string Name { get; set; }

        protected Shape(IRenderer renderer)
        {
          
            if (renderer == null)
            {
                throw new ArgumentNullException(nameof(renderer));
            }
            this.renderer = renderer;
        }

        // O ToString agora resolve dinamicamente para qualquer forma e qualquer renderizador
        public override string ToString()
        {
            return $"Drawing {Name} as {renderer.WhatToRenderAs}";
        }
    }

    // 4. Abstrações Refinadas (Não há mais VectorSquare, RasterTriangle, etc.)
    public class Triangle : Shape
    {
        public Triangle(IRenderer renderer) : base(renderer)
        {
            Name = "Triangle";
        }
    }

    public class Square : Shape
    {
        public Square(IRenderer renderer) : base(renderer)
        {
            Name = "Square";
        }
    }


    //============= Implementação incorreta (Resulta em duplicação cartesiana de produto) ==========================
//    public abstract class Shape
//    {
//      public string Name { get; set; }
//    }
//
//    public class Triangle : Shape
//    {
//      public Triangle() => Name = "Triangle";
//    }
//
//    public class Square : Shape
//    {
//      public Square() => Name = "Square";
//    }
//
//    public class VectorSquare : Square
//    {
//      public override string ToString() => "Drawing {Name} as lines";
//    }
//
//    public class RasterSquare : Square
//    {
//      public override string ToString() => "Drawing {Name} as pixels";
//    }
//
    // imagine VectorTriangle and RasterTriangle are here too
}

