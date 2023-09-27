import {Table} from "reactstrap";
import {Book} from "./Models/Book";

let data: any;

interface Props {
    books: Book[];
    handleUpdateBook: (book: Book) => void;
    handleDeleteBook: (book: Book) => void;
}

export default function ListBooks(props: Props) {
    data = props.books.map((book: Book) => {
        return (
            <tr key={book.isbn13 != "" ? book.isbn13 : book.isbn}>
                <td>{book.isbn13}</td>
                <td>{book.isbn}</td>
                <td>{book.title}</td>
                <td>{book.author}</td>
                <td>{book.yearPublished}</td>
                <td>{book.description}</td>
                <td>{book.genre}</td>
                <td>
                    <button onClick={() => props.handleUpdateBook(book)}>Update</button>
                    <button onClick={() => props.handleDeleteBook(book)}>Delete</button>
                </td>
            </tr>
        );
    });

    return (
        <div>
            <Table>
                <thead>
                <tr>
                    <th>ISBN13</th>
                    <th>ISBN</th>
                    <th>Title</th>
                    <th>Author</th>
                    <th>Publication year</th>
                    <th>Description</th>
                    <th>Genre</th>
                    <th>Actions</th>
                </tr>
                </thead>
                <tbody>
                {data}
                </tbody>
            </Table>
        </div>
    );
}