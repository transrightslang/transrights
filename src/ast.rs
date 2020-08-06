#[derive(Debug, Clone)]
pub struct Selector {
    pub selector: String,
    pub value: Box<Statement>
}
#[derive(Debug,Clone)]
pub enum Literal {
    Str(String)
}
#[derive(Debug, Clone)]
pub enum Statement {
    Literal(Literal),
    Message(String, Vec<Selector>),
    VarDeclaration(String, Box<Statement>),
    VarDefinition(String, Box<Statement>),
}