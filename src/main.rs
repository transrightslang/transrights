#[macro_use] extern crate lalrpop_util;

mod ast;

lalrpop_mod!(pub grammar);

fn main() {
    let parser = grammar::StatementListParser::new();
    let prog = r#"
        (Logger output:`data`);
        ok := ko;
    "#;
    let data = match parser.parse(prog) {
        Ok(val) => val,
        Err(val) => {
            println!("{:#?}", val);
            panic!();
        }
    };
    println!("{:#?}", data);
}
