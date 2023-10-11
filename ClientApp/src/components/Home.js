import React, {Component} from 'react';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div>
                <h1>Home</h1>
                <p>This is a project by Vilius Birštonas made with:</p>
                <ul>
                    <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for
                        cross-platform server-side code
                    </li>
                    <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
                    <li><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</li>
                </ul>
                <p>Currently, these are the features implemented:</p>
                <ul>
                    <li>Top quality book CRUD found under <em>Books</em></li>
                    <li>Link to the Book API Swagger found under <em>Swagger</em></li>
                    <li>Unit tests via NUnit</li>
                    <li>Use of an SQLite Database</li>
                    <li>ORM via Entity Framework</li>
                </ul>
            </div>
        );
    }
}
