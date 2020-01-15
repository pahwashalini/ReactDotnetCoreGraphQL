import React, { Component } from 'react';

export class FetchData extends Component {
    displayName = FetchData.name

    constructor(props) {
        super(props);
        this.state = { blogs: [], loading: true };

        fetch('api/Blogs/ListFromGraphql')
            .then(response => response.json())
            .then(data => {
                this.setState({ blogs: data, loading: false });
               
            });
        
    }

    static renderForecastsTable(blogs) {
        return (
            <table className='table'>
                <thead>
                    <tr>
                        <th>Blog ID</th>
                        <th>Title</th>
                        <th>Blog</th>
                        <th>Post Date</th>
                    </tr>
                </thead>
                <tbody>
                    {blogs.map(blog =>
                        <tr key={blog.blogPostID}>
                            <td>{blog.blogPostID}</td>
                            <td>{blog.title}</td>
                            <td>{blog.blogText}</td>
                            <td>{blog.postDate}</td>

                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderForecastsTable(this.state.blogs);

        return (
            <div>
                <h1>Weather blogs</h1>
                <p>this.state.blogs</p>
                {contents}
            </div>
        );
    }
}
