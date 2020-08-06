#[derive(Debug, Clone)]
pub struct Selector {
    pub selector: String,
    pub value: Box<Statement>
}
#[derive(Debug, Clone)]
pub enum Statement {
    Literal(String),
    Message(String, Vec<Selector>)
}