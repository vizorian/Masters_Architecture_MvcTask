// @ts-ignore
import CreateBookForm from "./CreateBookForm.tsx";
import {Book} from "./Models/Book";

const apiUrl = 'https://localhost:5000/api';

interface Props {
    handleCancel: () => void;
}

export default function CreateBook(props: Props) {

    return (
        <div>
            <h1>Create book</h1>
            <CreateBookForm handleSubmit={handleSubmit} handleCancel={props.handleCancel}/>
        </div>
    );

    function handleSubmit(book: Book) {
        fetch(`${apiUrl}/books`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(book)
        })
            .then(response => response.json())
            .then(() => {
                props.handleCancel();
            });
    }
}

