import fallbackImage from "@public/default.jpg";
import { FILE_BASE_URL } from "@env";

export const Image = ({ src, alt, className, ...rest }) => {
  const imageUrl = src ? FILE_BASE_URL + src : fallbackImage;
  return <img src={imageUrl} alt={alt} className={className} {...rest} />;
};
