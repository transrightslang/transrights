#[macro_use] extern crate lalrpop_util;

mod ast;

lalrpop_mod!(pub grammar);

fn main() {
    let parser = grammar::StatementListParser::new();
    let prog = r#"
    data := "yeet"
    (Logger print:data)
    "#;
    let data = parser.parse(prog).unwrap();
    println!("{:#?}", data);
}
