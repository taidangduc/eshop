import logo from "@public/logo-brand-no-bg.png";
import { Input } from "@components/ui";
import { Link } from "react-router-dom";

export function HeaderLayout() {
  return (
    <div className="bg-white">
      <div className="w-[1200px] mx-auto py-5 bg-white">
        <div className="flex flex-col">
          <div className="flex flex-row justify-between">
            <Link to="/">
              <img src={logo} width="162px" height="50px" />
            </Link>
            <div className="flex-1 mx-10 relative">
              <form
                role="search"
                autoComplete="off"
                className="h-10 w-full outline"
              >
                <div className="flex items-center h-full">
                  <div className="flex-1 focus-within:ring focus-within:ring-1 focus-within:ring-gray-900 ml-[-5px] mr-[5px]">
                    <input
                      type="text"
                      className="w-full h-full py-2 my-2 ml-3 pr-5 focus:outline-none"
                      width="50"
                    />
                  </div>

                  <button
                    type="button"
                    className="bg-gray-900 text-white px-5 h-full flex items-center justify-center"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="19"
                      height="19"
                      viewBox="0 0 24 24"
                    >
                      <path
                        fill="currentColor"
                        d="m18.031 16.617l4.283 4.282l-1.415 1.415l-4.282-4.283A8.96 8.96 0 0 1 11 20c-4.968 0-9-4.032-9-9s4.032-9 9-9s9 4.032 9 9a8.96 8.96 0 0 1-1.969 5.617m-2.006-.742A6.98 6
                        0 0 0 18 11c0-3.867-3.133-7-7-7s-7 3.133-7 7s3.133 7 7 7a6.98 6.98 0 0 0 4.875-1.975z"
                      />
                    </svg>
                  </button>
                </div>
              </form>
              <ul className="flex gap-2 absolute left-0 bottom-[-19px]">
                <li>
                  <a href="#1">#hashtag</a>
                </li>
                <li>
                  <a href="#1">#hashtag</a>
                </li>
                <li>
                  <a href="#1">#hashtag</a>
                </li>
              </ul>
            </div>
            <div
              className="flex flex-col my-auto relative mr-10"
              id="cart_target_id"
            >
              <div role="button">
                <div className="cart-btn-group ">
                  <a
                    href="/cart"
                    className="flex items-center relative"
                    style={{ marginLeft: "5px" }}
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="32"
                      height="32"
                      style={{
                        fontSize: "17px",
                        lineHeight: "20.4px",
                        marginRight: "10px",
                        display: "block",
                      }}
                      viewBox="0 0 24 24"
                    >
                      <path
                        fill="currentColor"
                        d="M11.25 18.75c0 .83-.67 1.5-1.5 1.5s-1.5-.67-1.5-1.5s.67-1.5 1.5-1.5s1.5.67 1.5 1.5m5-1.5c-.83 0-1.5.67-1.5 1.5s.67 1.5 1.5 1.5s1.5-.67 1.5-1.5s-.67-1.5-1.5-1.5m4.48-9.57l-2 8a.75.75 0 0 1-.73.57H8c-.36 0-.67-.26-.74-.62L5.37 5.25H4c-.41 0-.75-.34-.75-.75s.34-.75.75-.75h2c.36 0 .67.26.74.62l.43 2.38H20a.754.754 0 0 1 .73.93m-1.69.57H7.44l1.18 6.5h8.79z"
                      />
                    </svg>
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
