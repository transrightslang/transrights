#[macro_use] extern crate lalrpop_util;

mod ast;

lalrpop_mod!(pub grammar);

fn main() {
    let parser = grammar::StatementParser::new();
    let data = parser.parse("(object sel:(object sel:val) sel:(object sel: val))").unwrap();
    println!("{:#?}", data);
    assert!(parser.parse("(object sel:(object sel:val))").is_ok());
    assert!(!parser.parse("(object sel:(object sel:(val))").is_ok());
}
