// @ts-ignore
import UpdateBookForm from "./UpdateBookForm.tsx";
// @ts-ignore
import {BookDto} from "./Models/BookDto.tsx";
// @ts-ignore
import {Book} from "./Models/Book.tsx";

const apiUrl = 'https://localhost:5000/api';

interface Props {
    handleCancel: () => void;
    book: Book;
}

export default function UpdateBook(props: Props) {
    const bookDto: BookDto = {
        title: props.book.title,
        author: props.book.author,
        yearPublished: props.book.yearPublished,
        description: props.book.description,
        genre: props.book.genre
    };

    return (
        <div>
            <h1>Update book</h1>
            <UpdateBookForm handleSubmit={handleSubmit} handleCancel={props.handleCancel} book={bookDto}/>
        </div>
    );

    function handleSubmit(book: BookDto) {
        const bookId = props.book.isbn13 ? props.book.isbn13 : props.book.isbn;
        
        fetch(`${apiUrl}/books/${bookId}`, {
            method: 'PUT',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(book)
        })
            .then(() => {
                props.handleCancel();
            });
    }
}

