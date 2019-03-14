module Cacher

open System
open Giraffe
open Giraffe.GiraffeViewEngine

/// pre-render a node and return it as `rawText`.  Only use this outside of the closure of a function, meaning like so:
/// let myElement = cache <some complex stateless xmlNode>, or
///
/// let myElement =
///   let staticPart = cache <some complex stateless xmlnNde>
///   fun parameter1 -> <some xmlNode that uses the parameter 1 and embeds the staticPart>
///
///
/// this is so the same pre-rendered value can be reused.
let cache (node: XmlNode) =
    let cached = Giraffe.GiraffeViewEngine.renderHtmlNode node
    rawText cached

/// pre-render a node list and return it as `rawText`.  Only use this outside of the closure of a function, meaning like so:
/// let myElement = cache <some complex stateless xmlNode list>, or
///
/// let myElement =
///   let staticPart = cacheMany <some complex stateless xmlNode list>
///   fun parameter1 -> <some xmlNode that uses the parameter 1 and embeds the staticPart>
///
///
/// this is so the same pre-rendered value can be reused.
let cacheMany (nodes: XmlNode list) =
    let cached = Giraffe.GiraffeViewEngine.renderHtmlNodes nodes
    rawText cached