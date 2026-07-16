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