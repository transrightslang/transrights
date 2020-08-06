#[derive(Debug, Clone)]
pub struct Selector {
    pub selector: String,
    pub value: Box<Statement>
}
#[derive(Debug,Clone)]
pub enum Literal {
    Str(String),
}
#[derive(Debug,Clone)]
pub struct Argument {
    pub name: String,
    pub kind: String,
}
#[derive(Debug,Clone)]
pub struct Function {
    pub name: String,
    pub arguments: Vec<Argument>,
    pub replies: String,
    pub statements: Vec<Box<Statement>>,
}
#[derive(Debug, Clone)]
pub enum Statement {
    Literal(Literal),
    Ident(String),
    Message(String, Vec<Selector>),
    VarDeclaration(String, Box<Statement>),
    VarDefinition(String, Box<Statement>),
    Function(Function),
    NOOP,
}