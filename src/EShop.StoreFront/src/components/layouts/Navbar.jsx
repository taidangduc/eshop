import fallbackImage from "@public/default.jpg";
import { useAuth } from "@features/auth/context";
import { Link } from "react-router-dom";

export function NavbarLayout() {
  const { user, isAuthenticated, login, logout, isLoading } = useAuth();

  return (
    <div className="bg-white">
      <section className="w-[1200px] mx-auto bg-white">
        <div className="flex flex-col flex-1">
          <div className="flex flex-row flex-1 justify-end">
            <div className="flex flex-end">
              <ul className="flex items-center m-0">
                <li>
                  <Link to="/language" className="flex items-center">
                    <span>
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="16"
                        height="16"
                        viewBox="0 0 24 24"
                      >
                        <g fill="none" stroke="currentColor" strokeWidth="1.5">
                          <path d="M22 12a10 10 0 1 1-20.001 0A10 10 0 0 1 22 12Z" />
                          <path d="M16 12c0 1.313-.104 2.614-.305 3.827c-.2 1.213-.495 2.315-.867 3.244c-.371.929-.812 1.665-1.297 2.168c-.486.502-1.006.761-1.531.761s-1.045-.259-1.53-.761c-.486-.503-.927-1.24-1.298-2.168c-.372-.929-.667-2.03-.868-3.244A23.6 23.6 0 0 1 8 12c0-1.313.103-2.614.304-3.827s.496-2.315.868-3.244c.371-.929.812-1.665 1.297-2.168C10.955 2.26 11.475 2 12 2s1.045.259 1.53.761c.486.503.927 1.24 1.298 2.168c.372.929.667 2.03.867 3.244C15.897 9.386 16 10.687 16 12Z" />
                          <path strokeLinecap="round" d="M2 12h20" />
                        </g>
                      </svg>
                    </span>
                    <span className="pl-2">English</span>
                  </Link>
                </li>
                <li className="px-2">
                  {isAuthenticated ? (
                    <div className="flex items-center w-[100px] ml-2">
                      <Link to="/user" className="flex relative">
                        <div className="flex items-center py-1">
                          <span className="px-2">
                            {user?.userName || user?.name || "guest"}
                          </span>
                          <div className="block border-l border-gray-900 h-4 mx-1"></div>
                        </div>
                      </Link>
                      <a
                        onClick={() => logout()}
                        className="flex relative px-2 cursor-pointer"
                      >
                        Logout
                      </a>
                    </div>
                  ) : (
                    <a
                      onClick={() => login()}
                      className="flex justify-center items-center w-[100px] ml-2 cursor-pointer"
                    >
                      Login
                    </a>
                  )}
                </li>
              </ul>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}
