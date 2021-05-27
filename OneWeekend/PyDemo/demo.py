

compress_type = 'P3'
width = 256
height = 256
color_space = 255

path = 'Img1-ImageDemo.ppm'

header_string = '{}\n{} {}\n{}\n'.format(compress_type,width,height,color_space)
color_list = []

for i in range(0,width):
    for j in range(0,height):
        r = int(i * color_space / width)
        g = int(j * color_space / height)
        b = int(0.7 * color_space)
        color_list.append('{} {} {}'.format(r,g,b))

for i in color_list:
    header_string = '{}\n{}'.format(header_string, i)

with open(path,encoding='utf-8',mode='w') as f:
    f.write(header_string)